using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace PrintClient
{
    public partial class DialogFastReportDesign<TList,TEntity> : Form
    {

        List<TList> list;
        List<TEntity> entity;
        List<dynamic> listd;
        string filename;

        public DialogFastReportDesign(string filename,List<TList> list, List<TEntity> entity, List<dynamic> listd)
        {
            InitializeComponent();
            this.list = list;
            this.entity = entity;
            this.listd = listd;
            this.filename = filename;

        }

        private void DialogFastReportDesign_Load(object sender, EventArgs e)
        {
            this.TopMost = false;


            var report = new FastReport.Report();

            var path = Path.Combine(System.Environment.CurrentDirectory, "Report");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var reportFile = Path.Combine(System.Environment.CurrentDirectory, "Report/" + filename + ".frx");
            if (!File.Exists(reportFile))
            {
                designerControl1.Report = report;
                designerControl1.RefreshLayout();
                return;
            }

            report.RegisterData(list, "list");
            report.RegisterData(entity, "entity");
            report.RegisterData(listd, "chart");
            report.Load(reportFile);


            // 完成触发

            designerControl1.Report = report;
            designerControl1.RefreshLayout();
        }

        private void DialogFastReportDesign_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                
            }
            catch (Exception)
            {
            }
        }
    }
}
