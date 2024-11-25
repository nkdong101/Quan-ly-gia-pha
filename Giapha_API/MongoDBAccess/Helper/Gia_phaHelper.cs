using MongoDB.Driver;
using MongoDBAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoDBAccess
{
    /// <summary>
    /// Các hàm xử lý nghiệp vụ liên quan đến gia phả
    /// </summary>
    public class Gia_phaHelper : MongoDBAccess.DataAccess.MongoDB.ModelGenericRepository<Gia_pha>
    {
        /// <summary>
        /// Khởi tạo
        /// </summary>
        /// <param name="iUserId"></param>
        /// <param name="iClient">Kết nối</param>
        public Gia_phaHelper(uint? iUserId, MongoClient iClient = null) : base(iUserId, iClient)
        {
        }
        /// <summary>
        /// Đổi ngày âm dương
        /// </summary>
        /// <param name="iInfo"></param>
        /// <returns></returns>
        /// 

        public List<string> ThongBaoNgayGio()
        {
            List<string> notifications = new List<string>(); List<Gia_pha> vResult = new List<Gia_pha>();
            List<Gia_pha> list = this.Find().ToList();
            DateTime today = DateTime.Now;
            foreach (var item in list)
            {
                if (item.Date_of_death != null)
                {
                    for (int i = 0; i <= 7; i++) // Kiểm tra trong vòng 7 ngày tới
                    {
                        DateTime checkDate = today.AddDays(i);
                        DateTime NgayAM = DoilichAM_DUONG(checkDate);

                        // So sánh ngày giỗ (Date_of_death) với ngày âm lịch
                        if (item.Date_of_death.Value.Month == NgayAM.Month &&
                            item.Date_of_death.Value.Day == NgayAM.Day)
                        {
                            notifications.Add($"Ngày giỗ của <b>{item.Name}</b> là vào ngày <b>{NgayAM:dd/MM/yyyy}</b> ÂL (Dương lịch: <b>{checkDate:dd/MM/yyyy}</b>).");
                        }
                    }
                }
            }
            return notifications;
        }

        public List<string> Doilich(API_Input.Doi_lich iInfo)
        {
            
            List<string> vResult = new List<string>();
            var vLich = new Tuvi.Lichvannien();
            if (!iInfo.Amlich)
            {
                vResult.Add("Ngày " + iInfo.Day.ToString("dd/MM/yyyy") + " dương lịch tương ứng với: ");
                int[] vData = vLich.Duonglich_Amlich(iInfo.Day.Day, iInfo.Day.Month, iInfo.Day.Year);
                vResult.Add("ngày: " + new DateTime(vData[2], vData[1], vData[0]).ToString("dd/MM/yyyy"));
                vResult.AddRange(vLich.DayInfo(iInfo.Day.Day, iInfo.Day.Month, iInfo.Day.Year));
                if (vData[3] == 1)
                    vResult.Add("Tháng nhuận");
            }
            else
            {
                vResult.Add("Ngày " + iInfo.Day.ToString("dd/MM/yyyy") + " âm lịch" + (iInfo.Nhuan ? " tháng nhuận" : "") + " tương ứng với: ");
                int[] vData = vLich.Amlich_Duonglich(iInfo.Day.Day, iInfo.Day.Month, iInfo.Day.Year, iInfo.Nhuan ? 1 : 0);
                vResult.Add("ngày: " + new DateTime(vData[2], vData[1], vData[0]).ToString("dd/MM/yyyy"));
                vResult.AddRange(vLich.DayInfo(vData[0], vData[1], vData[2]));
            }

            return vResult;
        }

        //public string 

        #region GetData
        /// <summary>
        /// Lấy cây gia phả của dòng họ
        /// </summary>
        /// <param name="iDongho_id">ID dòng họ</param>
        /// <returns></returns>
        public Models.Extend.Giapha_Tree_Result GetTree()
        {
            uint iDongho_id = 1;
            var iConfig = new Models.Extend.Tree_box_Config();
            if (iConfig == null) iConfig = new Models.Extend.Tree_box_Config();
            var vPersonView = this.Find(p => p.User_id == this.UserId && (p.Dongho_id == iDongho_id)).FirstOrDefault();
            Models.Extend.Giapha_Tree_Result vResult = new Models.Extend.Giapha_Tree_Result();

            var vData = this.Find(p => p.Dongho_id == iDongho_id)
                           .SortBy(p => p.Level).ThenBy(p => p.Gender)
                           .ToList();
            if (vData.Count == 0) return vResult;
            // Xác định khu vực cần để vẽ cây gia phả
            var vGroup = vData.GroupBy(p => p.Level).Select(p => new Models.Extend.Level_Index()
            {
                Level = p.Key,
                Count = p.Count() + p.Where(a => a.Gender == Models.Enums.Gender.female).Sum(a => a.Couple.Count),
                Index = 0
            }).ToList().OrderByDescending(a => a.Count).ToList();
            var vMax = vGroup.FirstOrDefault();
            iConfig.MaxWidth = iConfig.X + vMax.Count * (iConfig.Width + iConfig.Distance + iConfig.Border * 2);
            iConfig.MaxHeight = iConfig.Y + vGroup.Count() * (iConfig.Height + iConfig.Distance + iConfig.Border * 2);
            vResult.Config = iConfig;
            // Xác định cụ tổ
            List<Models.Extend.Tree_Line> Lines = new List<Models.Extend.Tree_Line>();

            //vResult.Data = GetInfo(iDongho_id, vPersonView, vData[0], vData, ref vGroup, iConfig, vMax.Count, ref Lines);
            vResult.Data = DrawTree(iDongho_id, vPersonView, vData[0], vData, ref vGroup, iConfig, vMax, ref Lines);
            vResult.Lines = Lines;
            vResult.Data = vResult.Data.OrderBy(p => p.Level).ThenBy(p => p.Stt).ToList();
            return vResult;
        }
        /* 
        Nguyên tắc xử lý đi từ trên xuống dưới từ trái qua phải
        Nghĩa là đầu tiên xác định theo gốc (thủy tổ) sau đó đến các con của họ theo thứ tự con của bà cả xếp trước, các bà sau lần 
        lượt chứ không theo tuổi
        - Căn theo đời có nhiều con cháu nhất  tạm gọi là "đời làm mốc" (vì vị trí không thể vượt quá hàng này theo chiều ngang)
        - Các đời khác ít thành viên hơn sẽ căn vào chính giữa tuy nhiên điều này dẫn đến các nét vẽ có thể bị chồng chéo và dính vào nhau
        - Khi vẽ bố mẹ sẽ căn cứ vào vị trí của con cái và bố mẹ để tính toán
            + Nếu bố mẹ thuộc đời trước "đời làm mốc" thì bố mẹ căn vào chính giữa của con cái
              (tuy nhiên phải để ý đến người đứng trước, nếu vị trí sau khi căn khiến người đứng trước cùng hàng đẩy về sau thì phải 
              điều chỉnh lại vị trí của họ)
            + Nếu bố mẹ thuộc đời sau "đời làm mốc" thì con cái phải điều chỉnh sang ph
         */
        /// <summary>
        /// Lấy dữ liệu liên quan đến 1 người
        /// </summary>
        /// <param name="iDongho_id">ID dòng họ</param>
        /// <param name="iViewer">Người xem dữ liệu</param>
        /// <param name="iInfo">Người trong dòng họ đang cần lấy dữ liệu</param>
        /// <param name="iData">Danh sách người trong dòng họ</param>
        /// <param name="iIndex">Sử dụng để lấy stt theo từng đời</param>
        /// <param name="iConfig">Cấu hình để vẽ cây gia phả</param>
        /// <param name="iMaxIndex">Số thành viên lớn nhất trong 1 đời</param>
        /// <param name="Lines">Danh sách các đường nối vợ chồng con cái</param>
        /// <returns></returns>
        public List<Models.Extend.Giapha_Tree> GetInfo(uint iDongho_id,
                                                        Gia_pha iViewer,
                                                        Gia_pha iInfo,
                                                        List<Gia_pha> iData,
                                                        ref List<Models.Extend.Level_Index> iIndex,
                                                        Models.Extend.Tree_box_Config iConfig,
                                                        int iMaxIndex,
                                                        ref List<Models.Extend.Tree_Line> Lines
                                                         )
        {
            int vCount = 0;
            var vStt = iIndex.Where(p => p.Level == iInfo.Level).FirstOrDefault();
            if (vStt.Count == iMaxIndex)
                vCount = 0;
            else
                vCount = (iMaxIndex - vStt.Count) / 2;

            List<Models.Extend.Giapha_Tree> vResult = new List<Models.Extend.Giapha_Tree>();

            //1. Xử lý thông tin của con
            int? vSttMin = null;
            int vSttMax = 0;

            //1. Xóa bản ghi của chính mình vì kiểu gì mình cũng có trong danh sách rồi
            iData.Remove(iInfo);
            //2. Xử lý thông tin của vợ
            if (iInfo.Couple.Count == 0)// Không có vợ
            {
                var vNew = iInfo.ToTree(new Models.Extend.Tree_Box(iConfig, iInfo.Level, vStt.GetIndex(vCount)), iViewer);
                vResult.Add(vNew);
            }
            else if (iInfo.Couple.Count == 1)// Có 1 vợ thì vợ ở bên trái
            {
                var vNew = iInfo.ToTree(new Models.Extend.Tree_Box(iConfig, iInfo.Level, vStt.GetIndex(vCount)), iViewer);
                vResult.Add(vNew);

                var vCouple = iData.Where(p => p.Id == iInfo.Couple[0].Id).FirstOrDefault();
                if (vCouple == null)
                    vCouple = this.FindById(iInfo.Couple[0].Id);
                if (vCouple != null)
                {
                    vStt = iIndex.Where(p => p.Level == iInfo.Level).FirstOrDefault();
                    var vNewvCouple = vCouple.ToTree(new Models.Extend.Tree_Box(iConfig, iInfo.Level, vStt.GetIndex(vCount)), iViewer);
                    Lines.Add(new Models.Extend.Tree_Line()
                    {
                        Id1 = vNew.id,
                        Id2 = vCouple.Id,
                        Points = $"M{vNew.GetRight(iConfig).ToVSG()} {vNewvCouple.GetLeft(iConfig).ToVSG()}"
                    });
                    vResult.Add(vNewvCouple);
                    iData.Remove(vCouple);
                }
            }
            else// Nhiều vợ (chia đôi 1 nửa bên trái, 1 nửa bên phải)
            {
                int vDelta = iInfo.Couple.Count / 2;
                bool isAddMe = false;
                Models.Extend.Giapha_Tree vCurentBox = null;
                List<Models.Extend.Giapha_Tree> vListCouple = new List<Models.Extend.Giapha_Tree>();
                for (int i = 0; i < iInfo.Couple.Count; i++)
                {
                    if (i >= vDelta && !isAddMe)
                    {
                        vStt = iIndex.Where(p => p.Level == iInfo.Level).FirstOrDefault();
                        vCurentBox = iInfo.ToTree(new Models.Extend.Tree_Box(iConfig, iInfo.Level, vStt.GetIndex(vCount)), iViewer);
                        vResult.Add(vCurentBox);

                        isAddMe = true;
                    }
                    var vCouple = iData.Where(p => p.Id == iInfo.Couple[i].Id).FirstOrDefault();
                    if (vCouple != null)
                    {
                        vStt = iIndex.Where(p => p.Level == iInfo.Level).FirstOrDefault();
                        var vNewvCouple = vCouple.ToTree(new Models.Extend.Tree_Box(iConfig, iInfo.Level, vStt.GetIndex(vCount)), iViewer);
                        vListCouple.Add(vNewvCouple);
                        vResult.Add(vNewvCouple);
                        iData.Remove(vCouple);
                    }
                }
                // Xử lý đoạn nối vợ chồng
                foreach (var Item in vListCouple)
                {
                    if (Item.Stt > vCurentBox.Stt)
                        Lines.Add(new Models.Extend.Tree_Line()
                        {
                            Id1 = vCurentBox.id,
                            Id2 = Item.id,
                            Points = $"M{vCurentBox.GetRight(iConfig).ToVSG()} {Item.GetLeft(iConfig).ToVSG()}"
                        });
                    else
                        Lines.Add(new Models.Extend.Tree_Line()
                        {
                            Id1 = vCurentBox.id,
                            Id2 = Item.id,
                            Points = $"M{vCurentBox.GetLeft(iConfig).ToVSG()} {Item.GetRight(iConfig).ToVSG()}"
                        });
                }
            }
            if (iInfo.Childs.Count > 0)
            {
                var vList = iInfo.Childs.OrderBy(p => p.Index).ToList();
                foreach (var Item in vList)
                {
                    var vChild = iData.Where(p => p.Id == Item.Id).FirstOrDefault();
                    if (vChild == null)
                        vChild = this.FindById(Item.Id);
                    if (vChild != null)
                    {
                        var vListItem = GetInfo(iDongho_id, iViewer, vChild, iData, ref iIndex, iConfig, iMaxIndex, ref Lines);
                        if (vSttMin == null) vSttMin = vListItem[0].Stt;
                        if (vSttMax < vListItem[0].Stt) vSttMax = vListItem[0].Stt;
                        vResult.AddRange(vListItem);
                        // Tạo đường nối giữa bố mẹ và con
                        var vChildBox = vResult.Where(p => p.id == Item.Id).FirstOrDefault();
                        var vFatherBox = vResult.Where(p => p.id == vChildBox.fid).FirstOrDefault();
                        var vMotherBox = vResult.Where(p => p.id == vChildBox.mid).FirstOrDefault();
                        // Xác định tọa độ giữa của bố và mẹ
                        string vPoints = "M";
                        var vL1 = vFatherBox.Stt < vMotherBox.Stt ? vFatherBox.GetRight(iConfig) : vFatherBox.GetLeft(iConfig);
                        var vL2 = vFatherBox.Stt < vMotherBox.Stt ? vMotherBox.GetLeft(iConfig) : vMotherBox.GetRight(iConfig);
                        // Điểm bắt đầu và sổ thẳng xuống

                        double vY = (vL1.Lng + vL2.Lng) / 2 + iConfig.Height / 2 + iConfig.Distance / 2;
                        double vX = (vL1.Lat + vL2.Lat) / 2;
                        double vX_End = vChildBox.Box.X + iConfig.Width / 2;

                        vPoints += $"{vX},{(vL1.Lng + vL2.Lng) / 2} {vX},{vY}";
                        // Đường bo góc
                        vPoints += $" Q{vX},{vY + 5} {(vX < vX_End ? vX + 5 : vX - 5)},{vY + 5}";
                        // Nối đến tâm của box con
                        vPoints += $" L{vX_End},{vY + 5}";
                        // Bo góc trước khi nối vào box con
                        vPoints += $" Q{(vX < vX_End ? vX_End + 5 : vX_End - 5)},{vY + 5} {(vX < vX_End ? vX_End + 5 : vX_End - 5)},{vY + 10}";
                        // Nối vào Box con
                        vPoints += $" L{(vX < vX_End ? vX_End + 5 : vX_End - 5)},{vChildBox.Box.Y}";
                        Lines.Add(new Models.Extend.Tree_Line()
                        {
                            Id1 = vFatherBox.id,
                            Id2 = vChildBox.id,
                            Points = vPoints
                        });
                    }
                }
            }
            return vResult;
        }

        /// <summary>
        /// Vẽ cây gia phả
        /// </summary>
        /// <param name="iDongho_id"></param>
        /// <param name="iViewer"></param>
        /// <param name="iInfo"></param>
        /// <param name="iData"></param>
        /// <param name="iIndex"></param>
        /// <param name="iConfig"></param>
        /// <param name="iMaxIndex"></param>
        /// <param name="Lines"></param>
        /// <returns></returns>
        public List<Models.Extend.Giapha_Tree> DrawTree(uint iDongho_id,
                                                        Gia_pha iViewer,
                                                        Gia_pha iInfo,
                                                        List<Gia_pha> iData,
                                                        ref List<Models.Extend.Level_Index> iIndex,
                                                        Models.Extend.Tree_box_Config iConfig,
                                                        Models.Extend.Level_Index iBaseLevel,
                                                        ref List<Models.Extend.Tree_Line> Lines
                                                         )
        {
            List<Models.Extend.Giapha_Tree> vResult = new List<Models.Extend.Giapha_Tree>();

            var vStt = iIndex.Where(p => p.Level == iInfo.Level).FirstOrDefault();
            int vCoupleCount = iInfo.Couple.Count / 2;

            // Bản thân người này
            if (iInfo.Couple.Count == 0)// Không có vợ
            {
                var vMe = iInfo.ToTree(new Models.Extend.Tree_Box(iConfig, iInfo.Level, vStt.GetIndex(0)), iViewer);
                vResult.Add(vMe);
            }
            else
            {
                Models.Extend.Giapha_Tree vMe = null;
                List<Models.Extend.Giapha_Tree> vLines = new List<Models.Extend.Giapha_Tree>();
                // Xử lý thông tin của từng người vợ
                for (int i = 0; i < iInfo.Couple.Count; i++)
                {
                    var Item = iInfo.Couple[i];
                    // Lấy thông tin vợ/ chồng
                    var vCouple = iData.Where(p => p.Id == Item.Id).FirstOrDefault();
                    if (vCouple == null)
                        vCouple = this.FindById(Item.Id);
                    if (vCouple == null)
                    {
                        iInfo.Couple.Remove(Item);
                        this.Update(iInfo.Id, p => p.Set(a => a.Couple, iInfo.Couple), null, null);
                        vCoupleCount = iInfo.Couple.Count / 2;
                        i--;
                    }
                    else
                    {
                        // Xử lý thông tin con chung của 2 người
                        List<Models.Extend.Giapha_Tree> vChild_Box = new List<Models.Extend.Giapha_Tree>();
                        if (vCouple.Childs.Count > 0)
                        {
                            var vListChild = vCouple.Childs.OrderBy(p => p.Index).ToList();
                            for (int j = 0; j < vListChild.Count; j++)
                            {
                                // Nếu con chung mới hiển thị
                                if (iInfo.Childs.Where(p => p.Id == vListChild[j].Id).FirstOrDefault() != null)
                                {
                                    var vChild = iData.Where(p => p.Id == vListChild[j].Id).FirstOrDefault();
                                    if (vChild == null)
                                        vChild = this.FindById(vListChild[j].Id);
                                    if (vChild.Id == 7)
                                        Console.WriteLine("");
                                    if (vChild != null)
                                    {
                                        var vListItem = DrawTree(iDongho_id, iViewer, vChild, iData, ref iIndex, iConfig, iBaseLevel, ref Lines);
                                        if (vListItem.Count > 0)
                                        {
                                            var check = vListItem.Where(p => p.id == vChild.Id).FirstOrDefault();
                                            if (check != null)
                                                vChild_Box.Add(check);
                                            vResult.AddRange(vListItem);
                                        }
                                    }
                                }
                            }
                        }
                        // Box của bố/ mẹ
                        if (i >= vCoupleCount && vMe == null)
                        {
                            vMe = iInfo.ToTree(new Models.Extend.Tree_Box(iConfig, iInfo.Level, vStt.GetIndex(0)), iViewer);
                            foreach (var c in vLines)
                            {
                                Lines.Add(new Models.Extend.Tree_Line()
                                {
                                    Id1 = c.id,
                                    Id2 = vMe.id,
                                    Points = $"M{vMe.GetLeft(iConfig).ToVSG()} {c.GetRight(iConfig).ToVSG()}"
                                });
                            }
                        }
                        // Box của vợ/ chồng
                        if (iInfo.Id == 7)
                            Console.WriteLine("");
                        int vPoint = vChild_Box.Count == 0 ? 0 : (vChild_Box.Max(p => p.Stt) + vChild_Box.Min(p => p.Stt)) / 2;
                        var vCoupleBox = vCouple.ToTree(new Models.Extend.Tree_Box(iConfig, iInfo.Level, vStt.GetIndex(vPoint)), iViewer);
                        if (vChild_Box.Count > 0)
                        {
                            if (vStt.Index < vChild_Box.Max(p => p.Stt))
                                vStt.Index = vChild_Box.Max(p => p.Stt);
                            else if (vChild_Box[0].Level == iIndex.Max(p => p.Level))
                            {
                                // Khi mang nhớ mang theo 
                                int vReBegin = vStt.Index - vChild_Box.Count + 1;
                                foreach (var b in vChild_Box)
                                {
                                    b.Box.Re(iConfig, vReBegin);
                                    b.Stt = (byte)vReBegin;
                                    vReBegin += 1;
                                }
                                // Cập nhật lại index lớn nhất cho đời của con
                                var vStt_Child = iIndex.Where(p => p.Level == iInfo.Level + 1).FirstOrDefault();
                                if (vStt_Child != null)
                                    vStt_Child.Index = vStt.Index;
                            }
                        }
                        vResult.Add(vCoupleBox);
                        if (vMe != null && vChild_Box.Count > 0)
                            vMe = iInfo.ToTree(new Models.Extend.Tree_Box(iConfig, iInfo.Level, vCoupleBox.Stt - 1), iViewer);

                        // Tạo các đường nối với con
                        if (vChild_Box.Count > 0)
                        {
                            string vPoints = "M";
                            var vBegin = vMe == null ? vCoupleBox.GetRight(iConfig) : vCoupleBox.GetLeft(iConfig);
                            if (vMe == null)
                                vBegin.Lat += iConfig.Distance / 2;
                            else
                                vBegin.Lat -= iConfig.Distance / 2;
                            double Y_Down = vBegin.Lng + (double)iConfig.Height / 2 + (double)iConfig.Distance / 2;
                            vPoints += $"{vBegin.Lat},{vBegin.Lng} {vBegin.Lat},{Y_Down}";// Đoạn kéo thẳng xuống
                            foreach (var box in vChild_Box)
                            {
                                if (box.id == 94)
                                    Console.WriteLine("");
                                string vLineString = vPoints;
                                var vTop = box.Center_Top(iConfig);
                                bool isLeft = box.Stt < vCoupleBox.Stt;// True - Vẽ sang trái
                                // Đường bo góc
                                vLineString += $" Q{vBegin.Lat},{Y_Down + 5} {(isLeft ? vBegin.Lat - 5 : vBegin.Lat + 5)},{Y_Down + 5}";
                                // Nối đến tâm của box con
                                vLineString += $" L{vTop.Lat},{Y_Down + 5}";
                                // Bo góc trước khi nối vào box con
                                vLineString += $" Q{(isLeft ? vTop.Lat - 5 : vTop.Lat + 5)},{Y_Down + 5} {(isLeft ? vTop.Lat - 5 : vTop.Lat + 5)},{Y_Down + 10}";
                                // Nối vào Box con
                                vLineString += $" L{(isLeft ? vTop.Lat - 5 : vTop.Lat + 5)},{vTop.Lng}";
                                Lines.Add(new Models.Extend.Tree_Line()
                                {
                                    Id1 = vCoupleBox.id,
                                    Id2 = box.id,
                                    Points = vLineString
                                });
                            }

                        }
                        // Tạo đường nối với vợ/ chồng
                        if (vMe != null)// Nối sang trái
                        {
                            Lines.Add(new Models.Extend.Tree_Line()
                            {
                                Id1 = vCoupleBox.id,
                                Id2 = vMe.id,
                                Points = $"M{vMe.GetRight(iConfig).ToVSG()} {vCoupleBox.GetLeft(iConfig).ToVSG()}"
                            });
                        }
                        else
                            vLines.Add(vCoupleBox);
                    }
                }
                vResult.Add(vMe);
            }
            return vResult;
        }
        /// <summary>
        /// Lấy thông tin gia đình
        /// </summary>
        /// <param name="iCurent_id"></param>
        /// <returns></returns>
        public Models.Extend.Family GetFamily(uint iCurent_id)
        {
            Models.Extend.Family vResult = new Models.Extend.Family();
            var vInfo = this.FindById(iCurent_id);
            if (vInfo != null)
            {
                //1. Chính mình
                vResult.Curent = vInfo.Tofamily();
                //2. Bố mình
                if (vInfo.Parent != null && vInfo.Parent.Father_id != null)
                {
                    var vFather = this.FindById((uint)vInfo.Parent.Father_id);
                    if (vFather != null)
                        vResult.Farther = vFather.Tofamily();
                   
                }
                //3. Mẹ mình
                if (vInfo.Parent != null && vInfo.Parent.Mother_id != null)
                {
                    var vMother = this.FindById((uint)vInfo.Parent.Mother_id);
                    if (vMother != null)
                        vResult.Mother = vMother.Tofamily();
                   
                }
                //4. Anh chị em
                if (vInfo.siblings != null && vInfo.siblings.Count > 0)
                {
                    foreach (var Item in vInfo.siblings)
                    {
                        var vItem = this.FindById(Item.Id);
                        if (vItem != null)
                            vResult.Siblings.Add(vItem.Tofamily());
                    }
                }
                //5. Vợ con
                if (vInfo.Couple != null && vInfo.Couple.Count > 0)
                {
                    foreach (var Item in vInfo.Couple)
                    {
                        var vItem = this.FindById(Item.Id);
                        if (vItem != null)
                        {
                            var vNew = vItem.Tofamily();
                            if (vItem.Childs != null && vItem.Childs.Count > 0)
                            {
                                foreach (var Child in vItem.Childs)
                                {
                                    var vChild = this.FindById(Child.Id);
                                    if (vChild != null)
                                        vNew.Childs.Add(vChild.Tofamily());
                                }
                            }
                            vResult.Couple.Add(vNew);
                        }
                    }
                }
            }
            return vResult;
        }
        #endregion
        #region Thêm thông tin liên quan đến 1 thành viên
        /// <summary>
        /// Lưu thông tin bố mẹ
        /// </summary>
        /// <param name="iInfo"></param>
        public string Save_Bome(API_Input.Add_bome_Input iInfo)
        {
            var vCheck = this.FindById(iInfo.Con_Info.Id);
            if (vCheck == null)
                throw new Exception("Không tìm thấy thông tin của người thêm bố mẹ!");
            if (vCheck.Parent != null)
            {
                if (vCheck.Parent.Father_id > 0 && (iInfo.Bo_Info.Id == 0 || iInfo.Bo_Info.Id != vCheck.Parent.Father_id))
                    throw new Exception("Thông tin của bố không hợp lệ!");
                if (vCheck.Parent.Mother_id > 0 && (iInfo.Me_Info.Id == 0 || iInfo.Me_Info.Id != vCheck.Parent.Mother_id))
                    throw new Exception("Thông tin của mẹ không hợp lệ!");
            }
            using (var session = this.MongoClient.StartSession())
            {
                try
                {
                    session.StartTransaction();
                    bool isCreate_bo = false;
                    bool isCreate_me = false;
                    //0. Lấy Id và gán quan hệ
                    if (iInfo.Bo_Info.Id == 0)
                    {

                        iInfo.Bo_Info.Id = Global.GetSequance<Gia_pha>();
                        isCreate_bo = true;
                        iInfo.Bo_Info.Dongho_id = 1;
                        if (vCheck.Gender == Models.Enums.Gender.female)
                        {
                            var vHoBo = iInfo.Bo_Info.Name.Split(new char[] { ' ' });
                            if (vHoBo.Length > 0)
                            {
                                //var vHo = new Ho_VietNamHelper(1).Find(p => p.Name == Utility.TextManage.ChuHoaDau(vHoBo[0])).FirstOrDefault();
                                //if (vHo == null)
                                //{
                                //    throw new Exception("Rất tiếc, hệ thống không xác định được họ bố của " + vCheck.Name);
                                //}
                                //var vDongho_new = new Dong_ho()
                                //{
                                //    Ho_Vietnam_id = vHo.Id,
                                //    Name = Utility.TextManage.ChuHoaDau(vHoBo[0]),
                                //    Address = iInfo.Bo_Info.Noisinh
                                //};
                                //var vHo_id = new DonghoHelper(this.UserId, this.MongoClient).Create(vDongho_new, session);
                                iInfo.Bo_Info.Dongho_id = 1;
                                //this.Update(vCheck.Id, p => p.Set(a => a.Other_Name, vHo_id), session, null);
                            }
                        }
                    }
                    if (iInfo.Me_Info.Id == 0)
                    {
                        iInfo.Me_Info.Id = Global.GetSequance<Gia_pha>();
                        iInfo.Me_Info.Dongho_id = iInfo.Bo_Info.Dongho_id;
                        iInfo.Me_Info.Hongoai_id = 2;
                        isCreate_me = true;
                    }

                    iInfo.Bo_Info.Couple = new List<Models.Extend.Level>() { new Models.Extend.Level() { Cognition = false, Id = iInfo.Me_Info.Id, Index = 1 } };
                    iInfo.Me_Info.Couple = new List<Models.Extend.Level>() { new Models.Extend.Level() { Cognition = false, Id = iInfo.Bo_Info.Id, Index = 1 } };
                    //1. Thêm thông tin bố
                    Validate(iInfo.Bo_Info, session);
                    if (!isCreate_bo)
                        this.Update(iInfo.Bo_Info.Id, iInfo.Bo_Info.DefaultUpdateDefine(), session, null);
                    else
                        this.Create(iInfo.Bo_Info, session);
                    //2. Thêm thông tin mẹ
                    Validate(iInfo.Me_Info, session);
                    if (!isCreate_me)
                    {
                        iInfo.Me_Info.Hongoai_id = 2;
                        this.Update(iInfo.Me_Info.Id, iInfo.Me_Info.DefaultUpdateDefine(), session, null);

                    }
                    else
                        this.Create(iInfo.Me_Info, session);
                    //3. Cập nhật thông tin bố mẹ vào thông tin của con
                    var vPhahe = new Models.Extend.Pha_he()
                    {
                        Father_id = iInfo.Bo_Info.Id,
                        Mother_id = iInfo.Me_Info.Id
                    };
                    var vCurentChild = new Models.Extend.Level()
                    {
                        Cognition = true,
                        Id = vCheck.Id,
                        Index = vCheck.Index
                    };
                    if (iInfo.Bo_Info.Childs.Where(p => p.Id == vCurentChild.Id).FirstOrDefault() == null)
                        iInfo.Bo_Info.Childs.Add(vCurentChild);
                    if (iInfo.Me_Info.Childs.Where(p => p.Id == vCurentChild.Id).FirstOrDefault() == null)
                        iInfo.Me_Info.Childs.Add(vCurentChild);
                    this.Update(vCheck.Id, p => p.Set(a => a.Parent, vPhahe), session);
                    //4. Kiểm tra các anh chị em ruột, nếu có cập nhật lại thông tin bố mẹ cho họ
                    if (vCheck.siblings != null && vCheck.siblings.Count > 0)
                    {
                        var vData = vCheck.siblings.Where(p => p.Quanhe == Models.Enums.eQuanhe_ACE.ACE_Ruot).ToList();
                        foreach (var Item in vData)
                        {
                            var vItem = this.FindById(Item.Id);
                            if (vItem != null)
                            {
                                this.Update(vItem.Id, p => p.Set(a => a.Parent, vPhahe), session);
                                var vChild = new Models.Extend.Level()
                                {
                                    Cognition = true,
                                    Id = vItem.Id,
                                    Index = vItem.Index
                                };
                                if (iInfo.Bo_Info.Childs.Where(p => p.Id == vChild.Id).FirstOrDefault() == null)
                                    iInfo.Bo_Info.Childs.Add(vChild);
                                if (iInfo.Me_Info.Childs.Where(p => p.Id == vChild.Id).FirstOrDefault() == null)
                                    iInfo.Me_Info.Childs.Add(vChild);
                            }
                        }
                    }
                    //5. Cập nhật tất cả những người từ đời mình trở về sau đời đều tăng lên 1
                    if (vCheck.Level == 0)
                    {
                        var filter = Builders<Gia_pha>.Filter.Eq(p => p.Dongho_id, vCheck.Dongho_id);
                        filter &= Builders<Gia_pha>.Filter.Gte(p => p.Level, vCheck.Level);
                        var update = Builders<Gia_pha>.Update.Inc(p => p.Level, 1);
                        this.Collection.UpdateMany(session, filter, update, null);
                        this.Update(iInfo.Bo_Info.Id, p => p.Set(a => a.Level, 0), session);
                        this.Update(iInfo.Me_Info.Id, p => p.Set(a => a.Level, 0), session);
                    }

                    //6. Cập nhật lại thông tin bố mẹ với danh sách các con
                    this.Update(iInfo.Bo_Info.Id, p => p.Set(a => a.Childs, iInfo.Bo_Info.Childs), session);
                    this.Update(iInfo.Me_Info.Id, p => p.Set(a => a.Childs, iInfo.Me_Info.Childs), session);

                    session.CommitTransaction();
                }
                catch (Exception ex)
                {
                    session.AbortTransaction();
                    throw ex;
                }

            }
            return "OK";
        }
        /// <summary>
        /// Thêm mới anh chị em
        /// </summary>
        /// <param name="iInfo"></param>
        /// <returns></returns>
        public string Add_Anh_Chi_Em(API_Input.Add_ACE_Input iInfo)
        {
            var vCheck = this.FindById(iInfo.Curent_Info.Id);
            if (vCheck == null)
                throw new Exception("Không tìm thấy thông tin của người thêm anh chị em!");
            if (vCheck.siblings == null) vCheck.siblings = new List<Models.Extend.Anh_Chi_Em>();
            using (var session = this.MongoClient.StartSession())
            {
                try
                {
                    session.StartTransaction();

                    List<Models.Extend.Anh_Chi_Em> vListUpdate = new List<Models.Extend.Anh_Chi_Em>();
                    // 1. Thêm thông tin anh chị em vào gia phả
                    //1.1 Xác định bố mẹ chung của anh chị em
                    iInfo.ACE_Info.Level = vCheck.Level;
                    if (iInfo.ACE_Info.Id == 0)
                        iInfo.ACE_Info.Id = Global.GetSequance<Gia_pha>();
                    iInfo.ACE_Info.Parent = vCheck.Parent;


                    //

                    //1.2 Cập nhật con cái vào danh sách của bản ghi bố mẹ
                    if (vCheck.Parent.Father_id != null && vCheck.Parent.Father_id > 0)
                    {
                        var vBo = this.FindById((uint)vCheck.Parent.Father_id);
                        if (vBo != null)
                        {
                            if (vBo.Childs.Where(p => p.Id == iInfo.ACE_Info.Id).FirstOrDefault() == null)
                            {
                                this.Update(vBo.Id, p => p.Push(a => a.Childs, new Models.Extend.Level()
                                {
                                    Cognition = true,
                                    Id = iInfo.ACE_Info.Id,
                                    Index = iInfo.ACE_Info.Index
                                }), session, null);
                            }
                        }
                    }
                    if (vCheck.Parent.Mother_id != null && vCheck.Parent.Mother_id > 0)
                    {
                        var vMe = this.FindById((uint)vCheck.Parent.Mother_id);
                        if (vMe != null)
                        {
                            if (vMe.Childs.Where(p => p.Id == iInfo.ACE_Info.Id).FirstOrDefault() == null)
                            {
                                this.Update(vMe.Id, p => p.Push(a => a.Childs, new Models.Extend.Level()
                                {
                                    Cognition = true,
                                    Id = iInfo.ACE_Info.Id,
                                    Index = iInfo.ACE_Info.Index
                                }), session, null);
                            }
                        }
                    }
                    //1.3 Xác định các anh chị em liên quan
                    if (iInfo.ACE_Info.siblings == null) iInfo.ACE_Info.siblings = new List<Models.Extend.Anh_Chi_Em>();
                    if (vCheck.siblings != null && vCheck.siblings.Count > 0)
                    {
                        vListUpdate.AddRange(vCheck.siblings.Where(p => p.Id != iInfo.ACE_Info.Id).ToList());
                        // Anh chị em chung của 2 người
                        iInfo.ACE_Info.siblings = vCheck.siblings.Where(p => p.Id != iInfo.ACE_Info.Id).ToList();
                        // Thêm người hiện tại vào danh sách
                        iInfo.ACE_Info.siblings.Add(new Models.Extend.Anh_Chi_Em()
                        {
                            Id = vCheck.Id,
                            Index = vCheck.Index,
                            Quanhe = Models.Enums.eQuanhe_ACE.ACE_Ruot
                        });
                    }

                    iInfo.ACE_Info.siblings.Add(new Models.Extend.Anh_Chi_Em()
                    {
                        Id = vCheck.Id,
                        Index = vCheck.Index,
                        Quanhe = Models.Enums.eQuanhe_ACE.ACE_Ruot
                    });
                    Validate(iInfo.ACE_Info, session);

                    uint vId = this.Save(iInfo.ACE_Info, session, true);
                    // 2. Cập nhật lại danh sách anh chị em của người mới thêm
                    var vExit = vCheck.siblings.Where(p => p.Id == iInfo.ACE_Info.Id).FirstOrDefault();
                    if (vExit == null)
                    {
                        vCheck.siblings.Add(new Models.Extend.Anh_Chi_Em()
                        {
                            Id = vId,
                            Index = iInfo.ACE_Info.Index,
                            Quanhe = Models.Enums.eQuanhe_ACE.ACE_Ruot
                        });
                        this.Update(vCheck.Id, p => p.Set(a => a.siblings, vCheck.siblings), session);
                    }

                    //3. Kiểm tra update lại anh chị em của những người khác
                    foreach (var Item in vListUpdate)
                    {
                        var vItem = this.FindById(Item.Id);
                        if (vItem == null)
                        {
                            //session.AbortTransaction();
                            throw new Exception("Dữ liệu quan hệ không đúng, vui lòng liên hệ admin hệ thống!");
                        }
                        if (vItem.siblings == null) vItem.siblings = new List<Models.Extend.Anh_Chi_Em>();
                        vItem.siblings.Add(new Models.Extend.Anh_Chi_Em()
                        {
                            Id = vId,
                            Index = iInfo.ACE_Info.Index,
                            Quanhe = Item.Quanhe
                        });
                        this.Update(vItem.Id, p => p.Set(a => a.siblings, vItem.siblings), session);

                    }
                    session.CommitTransaction();
                }
                catch (Exception ex)
                {
                    session.AbortTransaction();
                    throw ex;
                }
            }
            return "OK";
        }
        /// <summary>
        /// Thêm mới con
        /// </summary>
        /// <param name="iInfo"></param>
        /// <returns></returns>
        public string Add_Con(API_Input.Add_Con_Input iInfo)
        {
            var vCheck = this.FindById(iInfo.Me_Info.Id);
            if (vCheck.Gender == Models.Enums.Gender.male)
                throw new Exception("Bạn chỉ có thể thêm con từ vị trí của người mẹ!");
            if (vCheck == null)
                throw new Exception("Không tìm thấy thông tin của người thêm bố mẹ!");
           
            if (vCheck.Couple == null || vCheck.Couple.Count == 0)
                throw new Exception("Rất tiếc, hệ thống không xác định được bố của " + iInfo.Con_Info.Name);
            using (var session = this.MongoClient.StartSession())
            {
                try
                {
                    session.StartTransaction();
                    List<uint> vListACE_Me = new List<uint>();
                    List<uint> vListACE_Cha = new List<uint>();

                    //1. Xác định phả hệ cho con và cập nhật thông tin
                    var vPhahe = new Models.Extend.Pha_he()
                    {
                        Father_id = vCheck.Couple[0].Id,
                        Mother_id = iInfo.Me_Info.Id
                    };
                    iInfo.Con_Info.Level = (byte)(vCheck.Level + 1);
                    iInfo.Con_Info.Parent = vPhahe;
                    Validate(iInfo.Con_Info, session);
                    uint vId = this.Create(iInfo.Con_Info, session);
                    //2. Cập nhật thông tin con vào bản ghi của bố mẹ
                    var vChild = new Models.Extend.Level() { Cognition = iInfo.Conrieng, Id = vId, Index = iInfo.Con_Info.Index };
                    //2.1 Bản ghi của mẹ
                    if (vCheck.Childs == null) vCheck.Childs = new List<Models.Extend.Level>();
                    vListACE_Me = vCheck.Childs.Select(p => p.Id).ToList();
                    vCheck.Childs.Add(vChild);
                    this.Update(vCheck.Id, p => p.Set(a => a.Childs, vCheck.Childs), session);
                    //2.2 bả ghi của bố
                    var vParent = this.FindById((uint)vPhahe.Father_id);
                    if (vParent == null)
                    {
                        session.AbortTransaction();
                        throw new Exception("Không tìm thấy thông tin của người bố!");
                    }
                    if (vParent.Childs == null) vParent.Childs = new List<Models.Extend.Level>();
                    vListACE_Cha = vCheck.Childs.Select(p => p.Id).ToList();
                    vParent.Childs.Add(vChild);
                    this.Update(vParent.Id, p => p.Set(a => a.Childs, vParent.Childs), session);
                    //3. Kiểm tra những anh chị em khác trong gia đình, add thêm vào danh sách
                    var vACE = this.Find(p => vListACE_Cha.Contains(p.Id) || vListACE_Me.Contains(p.Id)).ToList();
                    foreach (var Item in vACE)
                    {
                        bool vCheck_Cha = vListACE_Cha.Contains(Item.Id);
                        bool vCheck_Me = vListACE_Me.Contains(Item.Id);
                        Models.Enums.eQuanhe_ACE vQuanhe = (vCheck_Cha && vCheck_Me) ? Models.Enums.eQuanhe_ACE.ACE_Ruot :
                                                           vCheck_Cha ? Models.Enums.eQuanhe_ACE.Cung_Cha : Models.Enums.eQuanhe_ACE.Cung_me;
                        // Bản ghi hiện tại mới thêm
                        iInfo.Con_Info.siblings.Add(new Models.Extend.Anh_Chi_Em()
                        {
                            Id = Item.Id,
                            Index = Item.Index,
                            Quanhe = vQuanhe
                        });
                        // Bản ghi của anh chị em
                        Item.siblings.Add(new Models.Extend.Anh_Chi_Em()
                        {
                            Id = vId,
                            Index = iInfo.Con_Info.Index,
                            Quanhe = vQuanhe
                        });
                        this.Update(Item.Id, p => p.Set(a => a.siblings, Item.siblings), session);
                    }
                    this.Update(vId, p => p.Set(a => a.siblings, iInfo.Con_Info.siblings), session);

                    session.CommitTransaction();
                }
                catch (Exception ex)
                {
                    session.AbortTransaction();
                    throw ex;
                }
            }
            return "OK";
        }
        /// <summary>
        /// Thêm mới vợ/ chồng
        /// </summary>
        /// <param name="iInfo"></param>
        /// <returns></returns>
        public string Add_Vo(API_Input.Add_Vo_Input iInfo)
        {
            var vCheck = this.FindById(iInfo.Chong_Info.Id);
            if (vCheck == null)
                throw new Exception("Không tìm thấy thông tin của người chồng!");
            if (vCheck.Gender == iInfo.Vo_Info.Gender)
                throw new Exception("Giới tính vợ chồng không phù hợp, vui lòng kiểm tra lại!");

            iInfo.Vo_Info.Level = vCheck.Level;
            iInfo.Vo_Info.Couple = new List<Models.Extend.Level>()
            {
                new Models.Extend.Level()
                {
                    Cognition = false,
                    Id = vCheck.Id,
                    Index = 1
                }
            };
            Validate(iInfo.Vo_Info, null);
            using (var session = this.MongoClient.StartSession())
            {
                try
                {
                    session.StartTransaction();
                    //1. Cập nhật thông tin người vợ/ chồng
                    if (vCheck.Gender == Models.Enums.Gender.female)// Nữ trong họ đi lấy chồng
                    {
                        // Tạo họ dòng họ cho người chồng
                        var vHochong = iInfo.Vo_Info.Name.Split(new char[] { ' ' });
                        if (vHochong.Length > 0)
                        {
                            //var vHo = new Ho_VietNamHelper(1).Find(p => p.Name == Utility.TextManage.ChuHoaDau(vHochong[0])).FirstOrDefault();
                            //if (vHo == null)
                            //{
                            //    throw new Exception("Rất tiếc, hệ thống không xác định được họ chồng của " + vCheck.Name);
                            //}
                            //var vDongho_new = new Dong_ho()
                            //{
                            //    Ho_Vietnam_id = vHo.Id,
                            //    Name = Utility.TextManage.ChuHoaDau(vHochong[0]),
                            //    Address = iInfo.Vo_Info.Noisinh
                            //};
                            //var vHo_id = new DonghoHelper(this.UserId, this.MongoClient).Create(vDongho_new, session);
                            iInfo.Vo_Info.Hongoai_id = 2;
                            //this.Update(vCheck.Id, p => p.Set(a => a.Dongho_id, vHo_id).Set(a => a.Hongoai_id, vCheck.Dongho_id), session, null);
                        }
                    }
                    uint vVoId = this.Create(iInfo.Vo_Info, session);
                    //2. Cập nhật lại quan hệ của vợ chồng ở bản ghi người chồng
                    if (vCheck.Couple == null) vCheck.Couple = new List<Models.Extend.Level>();
                    vCheck.Couple.Add(new Models.Extend.Level()
                    {
                        Cognition = false,
                        Id = vVoId,
                        Index = iInfo.Vo_Info.Index
                    });
                    this.Update(vCheck.Id, p => p.Set(a => a.Couple, vCheck.Couple), session, null);
                    session.CommitTransaction();
                }
                catch (Exception ex)
                {
                    session.AbortTransaction();
                    throw ex;
                }
            }
            return "OK";
        }
        /// <summary>
        /// Kiểm tra dữ liệu trước khi cập nhật
        /// </summary>
        /// <param name="iInfo"></param>
        private void Validate(Gia_pha iInfo, IClientSessionHandle iSesstion)
        {
            //var vHelper = new DonghoHelper(this.UserId);
            //var vCheck = vHelper.FindById(iInfo.Dongho_id, iSesstion);
            //if (vCheck == null)
            //    throw new Exception("Xin vui lòng cho biết: " + iInfo.Name + " thuộc dòng họ nào?");
            //if (iInfo.HoVietNam_id == 0)
                //iInfo.HoVietNam_id = vCheck.Ho_Vietnam_id;
            if(iInfo.State == Models.Enums.State.Dead)
            {
                if(iInfo.year_die != null)
                {
                    iInfo.Date_of_death = DoilichAM_DUONG((DateTime)iInfo.Date_of_death);

                }
            }
            iInfo.Name = Utility.TextManage.ChuHoaDau(iInfo.Name);
        }
        /// <summary>
        /// Sửa thông tin
        /// </summary>
        /// <param name="iInfo"></param>
        /// <returns></returns>
        public string Edit(Gia_pha iInfo)
        {
            string result = this.Update(iInfo.Id, iInfo.DefaultUpdateDefine(), null);
            return result;
        }
        /// <summary>
        /// Xóa thông tin
        /// </summary>
        /// <param name="iInfo"></param>
        /// <returns></returns>
        public string Remove(uint iId)
        {
            var vCheck = this.FindById(iId);
            if (vCheck == null)
                throw new Exception("Không tìm thấy thông tin cần xóa!");
            if (vCheck.Childs != null && vCheck.Childs.Count > 0)
                throw new Exception("Người này đã có con, vui lòng xóa hết danh sách con trước khi xóa thông tin người này!");
            if (vCheck.Couple != null && vCheck.Couple.Count > 0)
            {
                var vCurent = new AccountHelper(this.UserId).FindById((uint)this.UserId);
                if (vCurent == null)
                    throw new Exception("Rất tiếc, chúng tôi không xác định được người thực hiện xóa dữ liệu!");
                //if (vCurent.Dongho_id == vCheck.Dongho_id)
                //{
                    if (vCheck.Gender == Models.Enums.Gender.female)
                        throw new Exception("Vui lòng xóa chồng của người này trước khi xóa thông tin người này!");
                    else
                        throw new Exception("Vui lòng xóa vợ của người này trước khi xóa thông tin người này!");
                //}
            }
            using (var session = this.MongoClient.StartSession())
            {
                try
                {
                    session.StartTransaction();
                    //1. Kiểm tra nếu bố mẹ có con này thì sẽ clear đi
                    if (vCheck.Parent != null)
                    {
                        //1.1 Bố
                        if (vCheck.Parent.Father_id != null)
                        {
                            var vParent = this.FindById((uint)vCheck.Parent.Father_id);
                            if (vParent != null && vParent.Childs != null)
                            {
                                var vChild = vParent.Childs.Where(p => p.Id == vCheck.Id).ToList();
                                if (vChild.Count > 0)
                                {
                                    foreach (var Item in vChild)
                                        vParent.Childs.Remove(Item);
                                    this.Update(vParent.Id, p => p.Set(a => a.Childs, vParent.Childs), session, null);
                                }
                            }
                        }
                        //1.2 Mẹ
                        if (vCheck.Parent.Mother_id != null)
                        {
                            var vMother = this.FindById((uint)vCheck.Parent.Mother_id);
                            if (vMother != null && vMother.Childs != null)
                            {
                                var vChild = vMother.Childs.Where(p => p.Id == vCheck.Id).ToList();
                                if (vChild.Count > 0)
                                {
                                    foreach (var Item in vChild)
                                        vMother.Childs.Remove(Item);
                                    this.Update(vMother.Id, p => p.Set(a => a.Childs, vMother.Childs), session, null);
                                }
                            }
                        }
                    }
                    //2. Kiểm ra vợ chồng
                    if (vCheck.Couple != null && vCheck.Couple.Count > 0)
                    {
                        foreach (var Item in vCheck.Couple)
                        {
                            var vData = this.FindById(Item.Id);
                            if (vData != null)
                            {
                                var vList = vData.Couple.Where(p => p.Id == vCheck.Id).ToList();
                                if (vList.Count > 0)
                                {
                                    foreach (var vCouple in vList)
                                        vData.Couple.Remove(vCouple);
                                    this.Update(vData.Id, p => p.Set(a => a.Couple, vData.Couple), session, null);
                                }
                            }
                        }
                    }
                    //3. Kiểm tra anh chị em
                    if (vCheck.siblings != null && vCheck.siblings.Count > 0)
                    {
                        foreach (var Item in vCheck.siblings)
                        {
                            var vData = this.FindById(Item.Id);
                            if (vData != null)
                            {
                                var vList = vData.siblings.Where(p => p.Id == vCheck.Id).ToList();
                                if (vList.Count > 0)
                                {
                                    foreach (var sibling in vList)
                                        vData.siblings.Remove(sibling);
                                    this.Update(vData.Id, p => p.Set(a => a.siblings, vData.siblings), session, null);
                                }
                            }
                        }
                    }
                    //4. Xóa thông tin
                    this.Delete(iId);
                    //5. Chốt số liệu
                    session.CommitTransaction();
                }
                catch (Exception ex)
                {
                    session.AbortTransaction();
                    throw ex;
                }
                return "OK";
            }
        }
        #endregion
        /// <summary>
        /// Xác định lại level của dòng họ
        /// </summary>
        /// <returns></returns>
        public string CheckLevel(int iDongho_id)
        {
            var vHo = new DonghoHelper(1).Find(p => p.Id == iDongho_id).ToList();
            foreach (var Item in vHo)
            {
                //1. Update tất cả level của dòng họ = 0
                var filter = Builders<Gia_pha>.Filter.Eq(p => p.Dongho_id, Item.Id);
                var update = Builders<Gia_pha>.Update.Set(p => p.Level, 0);
                this.Collection.UpdateMany(filter, update, null);
                //2. Kiểm tra cập nhật lại level cho từng đời
                var vData = this.Find(p => p.Dongho_id == Item.Id && p.Level == 0 && p.Gender == Models.Enums.Gender.male && (p.Parent == null || (p.Parent.Father_id == null || p.Parent.Mother_id == null))).FirstOrDefault();
                while (vData != null)
                {
                    UpdateLevel(vData, -1);
                    vData = this.Find(p => p.Dongho_id == Item.Id && p.Level == 0 && p.Gender == Models.Enums.Gender.male && (p.Parent == null || (p.Parent.Father_id == null || p.Parent.Mother_id == null))).FirstOrDefault();
                }
            }
            return "OK";
        }
        private bool UpdateLevel(Gia_pha iPerson, int iFather_Level)
        {
            iPerson.Level = (byte)(iFather_Level + 1);
            this.Update(iPerson.Id, p => p.Set(a => a.Level, iPerson.Level), null, null);
            if (iPerson.Childs != null && iPerson.Childs.Count > 0)
            {
                for (int i = 0; i < iPerson.Childs.Count; i++)
                {
                    var Item = iPerson.Childs[i];
                    var vInfo = this.FindById(Item.Id);
                    if (vInfo != null && vInfo.Dongho_id == iPerson.Dongho_id)
                    {
                        //this.Update(Item.Id, p => p.Set(a => a.Level, iPerson.Level + 1), null, null);
                        UpdateLevel(vInfo, iPerson.Level);
                    }
                    else
                    {
                        //iPerson.Childs.Remove(Item);
                        //this.Update(iPerson.Id, p => p.Set(a => a.Childs, iPerson.Childs), null, null);
                        //i--;
                    }
                }
            }
            if (iPerson.Couple != null && iPerson.Couple.Count > 0)
            {
                foreach (var Item in iPerson.Couple)
                    this.Update(Item.Id, p => p.Set(a => a.Level, iPerson.Level), null, null);
            }
            return true;
        }
        /// <summary>
        /// Gửi thông tin tài khoản cho người này
        /// </summary>
        /// <param name="iInfo"></param>
        public string SendAccount(Gia_pha iInfo)
        {
            var vInfo = this.FindById(iInfo.Id);
            if (vInfo == null)
                throw new Exception("Người này không có trong hệ thống, vui lòng kiểm tra lại hoặc liên hệ với hotline!");
            if (string.IsNullOrEmpty(vInfo.Phone))
                throw new Exception("Hệ thống đăng nhập bằng số điện thoại. Hãy cập nhật số điện thoại của " + vInfo.Name + " trước khi gửi!");
            var vRandom = new Random();
            var vUser = new User()
            {
                Images = vInfo.Avatar,
                BirthDay = vInfo.Birthday,
                CMND = vInfo.CCCD,
                //Dongho_id = vInfo.Dongho_id,
                Email = vInfo.Email,
                FullName = vInfo.Name,
                Phone = vInfo.Phone,
                UserCreate = (uint)this.UserId,
                Gender = vInfo.Gender,
                UserName = vInfo.Phone,
                Address = vInfo.Address,
                GroupPermission_Id = 3,
                Password = "VT" + vRandom.Next(100000, 999999),
                Giapha_id = vInfo.Id
            };
            using (var session = this.MongoClient.StartSession())
            {
                try
                {
                    session.StartTransaction();
                    // Khởi tạo tài khoản
                    var vUser_id = new AccountHelper(this.UserId, this.MongoClient).AddUser(null, vUser, session);
                    // Cập nhật kết nối tài khoản với gia phả
                    this.Update(vInfo.Id, p => p.Set(a => a.User_id, vUser_id), session, null);
                    session.CommitTransaction();
                }
                catch (Exception ex)
                {
                    session.AbortTransaction();
                    throw ex;
                }
            }
            return vUser.Password;
        }
        /// <summary>
        /// Tìm tổ tiên chung của 2 người
        /// </summary>
        /// <param name="iperson1"></param>
        /// <param name="person2"></param>
        /// <returns></returns>
        public Gia_pha To_tien_chung(Gia_pha iperson1, Gia_pha person2)
        {
            Gia_pha vResult = new Gia_pha();

            return vResult;
        }
    }
}