export default {
  Dashboard: {
    ForProcurment: [
      {
        type: 0, name: "MASTER LIST", background: "#795548", child: [
          {
            type: 1,
            name: "IN PROGRESS",
            background: "#409EFF",
          },
          { type: 2, name: "APPROVED", background: "#009688" },
          { type: 8, name: "Purchased", background: "#FF5722" },
          { type: 3, name: "REJECTED", background: "#673ab7" },
          {
            type: 4, name: "VOIDED", background: "#ebb563"
          },
        ]
      },
    ],
    ForOffice: [
      // { type: 0, name: "Action Required", background: "#00BCD4", child:[] },
      { type: 1, name: "Action Required", background: "#00BCD4" },
      { type: 2, name: "In Progress", background: "#3F51B5" },
      { type: 3, name: "Purchasing", background: "#FF5722" },
      { type: 4, name: "Other Status", background: "#ebb563" },

    ],
    ForEndorse: [
      {
        type: 1,
        name: "Submitted - Awaiting Review",
        background: "#409EFF",
      },
      { type: 2, name: "Endorsed", background: "#009688" },
      { type: 4, name: "WITHDRAWN", background: "#ebb563" },
      { type: 0, name: "MASTER LIST", background: "#795548" },
    ],
    ForApproval: [
      {
        type: 1,
        name: "Submitted - Awaiting Approval",
        background: "#409EFF",
      },
      { type: 2, name: "APPROVED", background: "#009688" },
      { type: 3, name: "REJECTED", background: "#673ab7" },
      { type: 4, name: "VOID", background: "#ebb563" },
      { type: 5, name: "PENDING VOID", background: "#9c27b0" },
      { type: 0, name: "MASTER LIST", background: "#795548" },
    ],
  }
}
