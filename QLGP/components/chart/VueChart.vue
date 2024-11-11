<template>
  <bar-chart
    class="chart"
    v-if="loaded && type == 'bar' && chartData"
    :chartOptions="chartOptions"
    :chartData="chartData"
  />
  <line-chart
    class="chart"
    v-else-if="loaded && type == 'line' && chartData"
    :chartOptions="chartOptions"
    :chartData="chartData"
  />
  <pie-chart
    class="chart"
    v-else-if="loaded && type == 'pie' && chartData"
    :chartOptions="chartOptions"
    :chartData="chartData"
  />
</template>

<script>
import Chart from "chart.js/auto";
import { fontString } from "chart.js/helpers";
import ConvertStr from "~/assets/scripts/ConvertStr";
function barAnimationComplete() {
  var chartInstance = this,
    ctx = chartInstance.ctx;

  ctx.font = fontString(
    Chart.defaults.font.size,
    Chart.defaults.font.style,
    Chart.defaults.font.family
  );
  ctx.textAlign = "center";
  ctx.textBaseline = "center";
  ctx.fillStyle = "#000000";
  this.data.datasets.forEach(function (dataset, i) {
    var meta = chartInstance.getDatasetMeta(i);
    meta.data.forEach(function (bar, index) {
      var data = ConvertStr.ToMoneyStr(dataset.data[index]);
      ctx.fillText(data, bar.x, bar.y - 10);
    });
  });
}

function pieAnimationComplete() {
  var chartInstance = this,
    ctx = chartInstance.ctx;
  ctx.font = fontString(
    Chart.defaults.font.size,
    "normal",
    Chart.defaults.font.family
  );
  ctx.textAlign = "center";
  ctx.textBaseline = "bottom";

  this.data.datasets.forEach(function (dataset) {
    // console.log(dataset);

    for (var i = 0; i < dataset.data.length; i++) {
      var model = chartInstance.getDatasetMeta(0).data[i],
        total = chartInstance.getDatasetMeta(0).total,
        mid_radius =
          model.innerRadius + (model.outerRadius - model.innerRadius) / 1.5,
        start_angle = model.startAngle,
        end_angle = model.endAngle,
        mid_angle = start_angle + (end_angle - start_angle) / 2;

      var x = mid_radius * Math.cos(mid_angle);
      var y = mid_radius * Math.sin(mid_angle);
      ctx.fillStyle = "#444";

      var percent = String(((dataset.data[i] / total) * 100).toFixed(1)) + "%";
      //Don't Display If Legend is hide or value is 0
      if (
        dataset.data[i] != 0
        //  &&                    dataset._meta[0].data[i].hidden != true
      ) {
        // Display percent in another line, line break doesn't work for fillText
        if (Math.round((dataset.data[i] / total) * 100) < 5) {
          // ctx.fillText(
          //   ConvertStr.ToMoneyStr(dataset.data[i]),
          //   model.x + x,
          //   model.y + y - 30
          // );
          ctx.fillText(percent, model.x + x, model.y + y - 15);
        } else {
          // ctx.fillText(
          //   ConvertStr.ToMoneyStr(dataset.data[i]),
          //   model.x + x,
          //   model.y + y
          // );
          ctx.fillText(percent, model.x + x, model.y + y + 15);
        }
      }
    }
  });
}
export default {
  props: {
    type: {
      type: String,
    },
    chartData: {},
    scales: {},
    legend: {},
  },
  data() {
    return {
      loaded: true,
      chartOptions: {
        responsive: true,
        maintainAspectRatio: false,
        events: [],
        tooltips: {
          // enabled: false,
        },
        hover: {
          animationDuration: 0,
        },
        plugins: {
          legend:
            this.legend !== false
              ? {
                  position: this.type == "pie" ? "left" : "top",
                }
              : null,
        },
        layout: {
          // padding: 50,
        },
        animation: {
          duration: 500,
          easing: "easeOutQuart",
          onComplete:
            this.type == "bar"
              ? function () {
                  barAnimationComplete.apply(this);
                }
              : this.type == "pie"
              ? pieAnimationComplete
              : null,
        },
        scales: this.scales || null,
      },
    };
  },
  watch: {
    chartData(newValue, oldValue) {
      this.loaded = false;
      this.$nextTick(() => {
        this.loaded = true;
      });
    },
  },
};
</script>

<style lang="scss" scoped>
.chart {
  width: 100%;
}
</style>
