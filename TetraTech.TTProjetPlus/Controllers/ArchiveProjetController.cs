using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;
using TetraTech.TTProjetPlus.Services;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class ArchiveProjetController : ControllerBase
    {
        private readonly HomeService _HomeService = new HomeService();
        private readonly FolderStructureService _FSService = new FolderStructureService();
        private readonly ArchiveService _ArchiveService = new ArchiveService();
        public static List<structureItem> ListeProjetsToArchive = new List<structureItem>();

        // GET: ArchiveProjet pour plusieurs projets a la fois
        public ActionResult Index(string displayRetour = "true", string menuVisible = "true")
        {
            DirectoryService obj = new DirectoryService();
            ViewBag.isAllowedAdministration = obj.isAllowedAdministration();
            ViewBag.MenuVisible = menuVisible;
            ListeProjetsToArchive = _HomeService.getListWSDR2(); //with structure de repertoire
            

            return View();
        }
        // GET: ArchiveProjet pour un projet a la fois
        public ActionResult Index2(string displayRetour = "true", string menuVisible = "true")
        {
            DirectoryService obj = new DirectoryService();
            ViewBag.isAllowedAdministration = obj.isAllowedAdministration();
            ViewBag.MenuVisible = menuVisible;
            ListeProjetsToArchive = _HomeService.getListWSDR(); //with structure de repertoire


            return View("index2");
        }

        public JsonResult returnServerUsed(string projectNumber)
        {
            var servers = _FSService.getServersUsed(projectNumber);
            return Json(new { Server = servers }, JsonRequestBehavior.AllowGet);

        }


        public ActionResult submitProjectToArchive(string projectId)
        {
            var resultF = new List<string>();
            try
            {
                
                var listProjects0 = projectId.Split(',').ToList();
                var listProjects = new List<string>();
                foreach (string item in listProjects0)
                {
                    if (item != null && item !="")
                    {
                        listProjects.Add(item);
                    }
                }
                var pathPartageList = _ArchiveService.returnPathPartage(listProjects);
                resultF = _ArchiveService.ArchiveProject(listProjects, pathPartageList);
                return Json(new { result = resultF }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _HomeService.addErrorLog(e,  "ArchiveProjet", "submitProjectToArchive", projectId);
                return Json(new { result = "fail" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult submitProjectToArchive2(string projectId)
        {
            try
            {
                var pathPartage = _ArchiveService.returnPathPartage2(projectId);
                var resultF = _ArchiveService.ArchiveProject2(projectId, pathPartage);
                return Json(new { result = resultF }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _HomeService.addErrorLog(e, "ArchiveProjet", "submitProjectToArchive", projectId);
                return Json(new { result = "fail" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}