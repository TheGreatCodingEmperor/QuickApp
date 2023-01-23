using System.Reflection;
using CSharpToTypeScript.Core.DependencyInjection;
using CSharpToTypeScript.Core.Services.TypeConversionHandlers;
using CSharpToTypeScript.Core.Options;
using CSharpToTypeScript.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using QuickApp.Controllers;
using QuickApp.Test.Helper;
using QuickApp.ViewModels;

namespace QuickApp.Test;

public class TsConverterTest {
    private ICodeConverter _codeConverter {get;set;}
    [SetUp]
    public void Setup () {  
        var services = new ServiceCollection();
        services.AddCSharpToTypeScript();

        var serviceProvider = services.BuildServiceProvider();

        _codeConverter = serviceProvider.GetService<ICodeConverter>();
    }

    // [TestCase (-1)]
    // [TestCase (0)]
    [TestCase (1)]
    [Test]
    public void Test1 (int value) {
        string result = _codeConverter.ConvertToTypeScript(@"class Item
{
    public Product product {get;set;}
    public int? Id { get; set; }
    public List<string> Name { get; set; }
}",new CodeConversionOptions(export: true, useTabs: false, tabSize: 4));
        Assert.IsTrue (value >= 0, "1 should not be prime");
    }
    
}