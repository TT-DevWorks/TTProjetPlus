using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Services;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class ProjetController : ControllerBase
    {
        private readonly DashBoardService _DashboardService = new DashBoardService();

        // GET: Projet
        public ActionResult Index()
        {
            ViewData["CurrentPeriodWeekDate"] = _DashboardService.GetCurrentPeriodWeekDate();

            ListOfYourProjectModel ListProjects = new ListOfYourProjectModel();
            ListProjects.ListOfProjects = new List<YourProjectsModel>();
            for (int i = 0; i < 50; i++)
            {
                YourProjectsModel model1 = new YourProjectsModel();
                model1.ProjectID = "715";
                model1.ProjectNumber = RandomString(4);
                model1.ProjectTitle = RandomString(3);
                model1.ProjectManager = RandomString(4);
                model1.Revenu = returnNumber();
                model1.Cost = returnNumber(); ;
                model1.Client = RandomString(4);

                ListProjects.ListOfProjects.Add(model1);
            }

            return View(ListProjects);
        }
        public int returnNumber()
        {
            return random.Next(100000);
        }
        private static Random random = new Random((int)DateTime.Now.Ticks);//thanks to McAden	
        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
    }
}