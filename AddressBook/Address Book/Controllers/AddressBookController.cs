using Address_Book.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace Address_Book.Controllers
{
    public class AddressBookController : Controller
    {
        List<AddressBookViewModel> models = new List<AddressBookViewModel>();
        JSONReadWrite readWrite = new JSONReadWrite();

        public AddressBookController()
        {
            giveID(models);
            models = JsonConvert.DeserializeObject<List<AddressBookViewModel>>(readWrite.Read("addressbook.json", "data"));
        }
        public void giveID(List<AddressBookViewModel> models)
        {
            int idCount = 1;
            models = JsonConvert.DeserializeObject<List<AddressBookViewModel>>(readWrite.Read("addressbook.json", "data"));
            foreach (var m in models)
            {
                m.Id = idCount;
                idCount = idCount + 1;
            }
            string jSONString = JsonConvert.SerializeObject(models);
            readWrite.Write("addressbook.json", "data", jSONString);
        }
        public IActionResult Index()
        {
            return View(models);
        }
    }





    public class JSONReadWrite
    {
        public JSONReadWrite() { }

        public string Read(string fileName, string location)
        {
            string root = "wwwroot";
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                root,
                location,
                fileName);

            string jsonResult;

            using (StreamReader streamReader = new StreamReader(path))
            {
                jsonResult = streamReader.ReadToEnd();
            }
            return jsonResult;
        }

        public void Write(string fileName, string location, string jSONString)
        {
            string root = "wwwroot";
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                root,
                location,
                fileName);

            using (var streamWriter = File.CreateText(path))
            {
                streamWriter.Write(jSONString);
            }
        }
    }
}
