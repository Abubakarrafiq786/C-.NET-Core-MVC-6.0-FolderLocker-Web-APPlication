using System.Security.AccessControl;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Principal;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class FolderlockerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LockFolder(string folderPath)
        {
            LockUnlockFolder(folderPath, true);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UnlockFolder(string folderPath)
        {
            LockUnlockFolder(folderPath, false);
            return RedirectToAction("Index");
        }

        private void LockUnlockFolder(string folderPath, bool isLock)
        {
            try
            {
                string adminUserName = Environment.UserName;
                DirectoryInfo dInfo = new DirectoryInfo(folderPath);
                DirectorySecurity ds = dInfo.GetAccessControl();

                FileSystemAccessRule fsa = new FileSystemAccessRule(adminUserName, FileSystemRights.FullControl, AccessControlType.Deny);
                ds.AddAccessRule(new FileSystemAccessRule(adminUserName, FileSystemRights.FullControl, AccessControlType.Deny));
                if (isLock)
                {
                    ds.AddAccessRule(fsa);
                    ViewBag.Status = "Folder Locked";
                }
                else
                {
                    ds.RemoveAccessRule(fsa);
                    ViewBag.Status = "Folder Unlocked";
                }

                dInfo.SetAccessControl(ds);
            }
            catch (Exception ex)
            {
                ViewBag.Status = $"Error: {ex.Message}";
            }
        }


        [HttpPost]
        public IActionResult UploadFiles(string folderPath, IFormFile file)
        {
            UploadFile(folderPath, file);
            return RedirectToAction("Index");
        }

        private void UploadFile(string folderPath, IFormFile file)
        {
            try
            {
                string adminUserName = Environment.UserName;
                DirectoryInfo dInfo = new DirectoryInfo(folderPath);
                DirectorySecurity ds = dInfo.GetAccessControl();

                FileSystemAccessRule fsa = new FileSystemAccessRule(adminUserName, FileSystemRights.FullControl, AccessControlType.Deny);
                ds.AddAccessRule(new FileSystemAccessRule(adminUserName, FileSystemRights.FullControl, AccessControlType.Deny));
                ds.RemoveAccessRule(fsa);
                dInfo.SetAccessControl(ds);
                if (file.Length > 0)
                {
                    string filename = Path.Combine(folderPath, file.FileName);
                    using (var filestream = new FileStream(filename, FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }


                }
                ds.AddAccessRule(fsa);
                dInfo.SetAccessControl(ds);
                ViewBag.Status = "File Uploaded successfully.";

            }
            catch (Exception ex)
            {
                ViewBag.Status += $"Error uploading file: {ex.Message}";
            }
        }
        public IActionResult delete()
        {
            return View();
        }


    }

}
