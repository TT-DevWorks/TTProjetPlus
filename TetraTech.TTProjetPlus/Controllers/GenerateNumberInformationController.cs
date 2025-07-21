using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class GenerateNumberInformationController : ControllerBase
    {
        public static IEnumerable<Color> Colors = new List<Color> {
    new Color {
        ColorId = 1,
        Name = "Red"
    },
    new Color {
        ColorId = 2,
        Name = "Blue"
       }
    };



        public static List<Letter> Letters = returnLetterList();

        public static List<Letter> returnLetterList()
        {
            List<Letter> list = new List<Letter>();
            for (char c = 'A'; c <= 'Z'; c++)
            {
                list.Add(new Letter
                {
                    LetterName = c,
                    Value = c
                });
            }
            return list;
        }


        public static string masterProjectPanelState = "";

        public ActionResult Index()
        {

            numberGenerationModel model = new numberGenerationModel();
            //DataSet ds = new DataSet();
            //DataTable dataTable = new DataTable();
            //var con = new SqlConnection("Data Source=qc1s196;Initial Catalog=Vortex;User ID=UserVortex;Password=xetrovvortex;");
            //var com = new SqlCommand();
            //com.Connection = con;
            //com.CommandType = CommandType.StoredProcedure;
            //com.CommandText = "[usp_Search_MasterProjectNumbers]";

            //var adapt = new SqlDataAdapter();
            //adapt.SelectCommand = com;
            //adapt.Fill(dataTable);
            //dataTable.TableName = "records";

            //con.Close();
            //com.Dispose();

            //if (dataTable != null && dataTable.Rows.Count > 0)
            //{
            //    foreach (DataRow row in dataTable.Rows)
            //    {
            //        AVAHealthModel record = new AVAHealthModel();

            //        record.bus = row["Bus"].ToString().TrimStart().TrimEnd();
            //        record.count = (int)row["EventsCount"];
            //        results.Add(record);
            //    }
            //}
            return View();
        }

        [HttpPost]
        public string changeMasterProjectPanelSate(string state)
        {
            masterProjectPanelState = state;
            return masterProjectPanelState;
        }

        public ActionResult submitAction(numberGenerationModel model)
        {
            //GNP = Generation Numero Projet
            return View("resumeGNP", model);
        }


        public ActionResult returnToIndex(numberGenerationModel model)
        {
            return View();
        }
    }
}
