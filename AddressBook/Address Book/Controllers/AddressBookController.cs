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
        /// <summary>
        /// A List of objects of the model
        /// </summary>
        List<AddressBookViewModel> models = new List<AddressBookViewModel>();
        JSONReadWrite readWrite = new JSONReadWrite();
        /// <summary>
        /// Constructor that give id to each of the person, and deserialize the json file data
        /// </summary>
        public AddressBookController()
        {
            giveID(models);
            models = JsonConvert.DeserializeObject<List<AddressBookViewModel>>(readWrite.Read("addressbook.json", "data"));
        }
        /// <summary>
        /// A function to give Id to each of the person in json file data
        /// </summary>
        /// <param name="models"></param>
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
        /// <summary>
        /// Function to add the data, from input form, and write it to json file
        /// </summary>
        /// <param name="addressBookViewModel"></param>
        /// <returns></returns>
        public IActionResult AddAddress(AddressBookViewModel addressBookViewModel)
        {
            models = JsonConvert.DeserializeObject<List<AddressBookViewModel>>(readWrite.Read("addressbook.json", "data"));
            models.Add(addressBookViewModel);
            string jSONString = JsonConvert.SerializeObject(models);
            readWrite.Write("addressbook.json", "data", jSONString);
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Function to Delete the record, when the button is pressed. First delete it from the list, then rewrite the json file
        /// </summary>
        /// <param name="id2del"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Function to Edit the existing data, when the button is pressed, then rewrite the new data into json file when button is submitted
        /// </summary>
        /// <param name="model"></param>
        /// <returns> return to index page </returns>
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
        /// <summary>
        /// Read Json File
        /// </summary>
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
        /// <summary>
        /// ReWrite text into Json File
        /// </summary>

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
