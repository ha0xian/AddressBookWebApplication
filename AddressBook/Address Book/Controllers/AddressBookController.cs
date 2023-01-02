using Microsoft.AspNetCore.Mvc;
using Address_Book.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Reflection;


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

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult AddAddress(AddressBookViewModel addressBookViewModel)
        {
            models = JsonConvert.DeserializeObject<List<AddressBookViewModel>>(readWrite.Read("addressbook.json", "data"));
            models.Add(addressBookViewModel);
            string jSONString = JsonConvert.SerializeObject(models);
            readWrite.Write("addressbook.json", "data", jSONString);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Delete(int id2del)
        {
            models.RemoveAt(id2del - 1);
            giveID(models);
            string jSONString = JsonConvert.SerializeObject(models);
            readWrite.Write("addressbook.json", "data", jSONString);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var model = models.FirstOrDefault(m => m.Id == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(AddressBookViewModel model)
        {
            var existingModel = models.FirstOrDefault(m => m.Id == model.Id);
            existingModel.first_name = model.first_name;
            existingModel.last_name = model.last_name;
            existingModel.phone = model.phone;
            existingModel.email = model.email;
            string jSONString = JsonConvert.SerializeObject(models);
            readWrite.Write("addressbook.json", "data", jSONString);
            return RedirectToAction("Index");
        }

        public int getLength()
        {
            return models.Count(); 
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
