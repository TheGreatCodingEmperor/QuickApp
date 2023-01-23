using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using QuickApp.Controllers;
using QuickApp.Test.Helper;
using QuickApp.ViewModels;

namespace QuickApp.Test;

public class ModelTsGenerator {
    [SetUp]
    public void Setup () { }

    // [TestCase (-1)]
    // [TestCase (0)]
    [TestCase (1)]
    [Test]
    public void Test1 (int value) {
        Type model = typeof (ProductViewModel);
        PropertyInfo[] properties = model.GetProperties();
        List<string> propertyStrs = new List<string>();
        foreach(PropertyInfo propertyInfo in properties){
            string name = Utilities.lowerCaseFirstChar(propertyInfo.Name);
            string propertyStr =  $"{name}:{Utilities.ConvertToTypescriptType(propertyInfo.PropertyType)};\r\n";
            propertyStrs.Add(propertyStr);
        }
        string modelStr = @$"
        export class {model.Name}{{
            {string.Join("",propertyStrs)}
        }}
        ";
        Console.Write(modelStr);

        Assert.IsTrue (value >= 0, "1 should not be prime");
    }
    private void ExportTsClassFile(string filePath,string content){

    }
    
}