using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SignalRCallingSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> FileUpload(IFormFile iFile)
        {

            var dict = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());

            String group = dict["group"];
            if (group != null)
            {

                string directoryOther = Path.GetPathRoot(Environment.CurrentDirectory) + "TelehealthFiles\\" + group;
                if (!Directory.Exists(directoryOther))
                {
                    Directory.CreateDirectory(directoryOther);
                }
                try
                {
                    var file = Request.Form.Files[0];

                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(directoryOther, fileName);
                        var dbPath = Path.Combine(directoryOther, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        return Ok(new { dbPath });

                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex}");
                }
            }


            return Ok("wrong input");
        }
        [HttpGet]
        public async Task<IActionResult> Download(string FilePath)
        {
            //if (filename == null)
            //    return Content("filename not present");

            //var path = Path.Combine(
            //               Directory.GetCurrentDirectory(),
            //               "wwwroot", filename);
            var path = FilePath;

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        //[HttpGet]
        //[Route("Download")]
        //public IActionResult DownloadSingleFile(string FilePath)
        //{
        //    HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
        //    //string FilePath = _httpContextAccessor.HttpContext.Request.Headers["FilePath"];
        //    if (!string.IsNullOrEmpty(FilePath))
        //    {
        //        try
        //        {
        //            string fileName = Path.GetFileName(FilePath);

        //            if (!string.IsNullOrEmpty(fileName))
        //            {
        //             //   FilePath = _commonservice.GetMappedFilePath(("~/" + FilePath));

        //                using (MemoryStream ms = new MemoryStream())
        //                {
        //                    using (FileStream file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
        //                    {
        //                        byte[] bytes = new byte[file.Length];
        //                        file.Read(bytes, 0, (int)file.Length);
        //                        ms.Write(bytes, 0, (int)file.Length);
        //                        httpResponseMessage.Content = new ByteArrayContent(bytes.ToArray());
        //                        httpResponseMessage.Content.Headers.Add("x-filename", fileName);
        //                        httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        //                        httpResponseMessage.StatusCode = HttpStatusCode.OK;
        //                        ms.Close();
        //                        return Ok(httpResponseMessage);
        //                    }
        //                }
        //            }
        //            return Ok("File not found.");
        //        }
        //        catch (Exception ex)
        //        {
        //            return Ok(ex);
        //        }
        //    }
        //    return Ok("Path not found.");
        //}



        //public HttpResponseMessage DownloadSingleFile(string path)
        //{
        //    HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
        //    string FilePath = path;
        //    string fileName = "";
        //    string downloadPath = "";
        //    if (!string.IsNullOrEmpty(FilePath))
        //    {
        //        try
        //        {
        //            if (string.IsNullOrEmpty(FileType))
        //            {
        //                fileName = Path.GetFileName(FilePath);
        //                FileType = fileName.Split('.')[1];
        //            }
        //            else
        //            {
        //                //special case scenario: PTL/SSCM
        //                fileName = CompleteFilePath;

        //                //check if file exists on 10.10.30.50
        //                downloadPath = path;//getFileFromTalkEHRServer(FilePath, FileType);
        //                if (string.IsNullOrEmpty(downloadPath))
        //                {
        //                    //check if file exists on filesrv1, if exists, file is copied from filesrv1 to 10.10.30.50 (FULL ACCESS RIGHTS ARE REQUIRED)
        //                    downloadPath = path; //getFileFromFileSRV(FilePath, FileType);
        //                    if (string.IsNullOrEmpty(downloadPath))
        //                    {
        //                        //check if file exists on bfilesrv1, if exists, file is copied from bfilesrv1 to filesrv1 (FULL ACCESS RIGHTS ARE REQUIRED)
        //                        downloadPath = path;//getFileFromBFileSRV(FilePath, FileType);
        //                    }
        //                }
        //            }

        //            if (!string.IsNullOrEmpty(fileName))
        //            {

        //            //        FilePath = downloadPath;

        //                using (MemoryStream ms = new MemoryStream())
        //                {
        //                    using (FileStream file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
        //                    {
        //                        byte[] bytes = new byte[file.Length];
        //                        file.Read(bytes, 0, (int)file.Length);
        //                        ms.Write(bytes, 0, (int)file.Length);
        //                        httpResponseMessage.Content = new ByteArrayContent(bytes.ToArray());
        //                        httpResponseMessage.Content.Headers.Add("x-filename", fileName);
        //                        if (FileType == "pdf")
        //                            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        //                        else if (FileType == ".mp3")
        //                            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("audio/mpeg");

        //                        else
        //                            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        //                        httpResponseMessage.StatusCode =System.Net.HttpStatusCode.OK;
        //                        ms.Close();
        //                        return httpResponseMessage;


        //                    }
        //                }
        //            }
        //            return null;
        //        }
        //        catch (Exception ex)
        //        {

        //            return null;
        //        }
        //    }
        //    return null;
        //}
    }
}
