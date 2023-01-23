using System.Reflection;
using CSharpToTypeScript.Core.DependencyInjection;
using CSharpToTypeScript.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using QuickApp.Controllers;
using QuickApp.Test.Helper;

namespace QuickApp.Test;

public class Tests {
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
        Type controller = typeof (ProductController);
        string angularServiceString = ConvertToAngularService(controller);
        Console.WriteLine(angularServiceString);
        Assert.IsFalse (value >= 0, "1 should not be prime");
    }
    private string ConvertToAngularService(Type controller){
 IEnumerable<AngularServiceUnit> angularServiceUnits = GetAngularServiceUnits (controller);
        string serviceTs = @$"
import {{ HttpClient }} from ""@angular/common/http"";
import {{ Injectable }} from ""@angular/core"";
import {{ TreeNode }} from ""primeng/api"";
import {{ Observable }} from ""rxjs"";
import {{ catchError }} from ""rxjs/operators"";
import {{ AuthService }} from ""src/app/services/auth.service"";
import {{ EndpointBase }} from 'src/app/services/endpoint-base.service';
import * as _ from 'lodash';

@Injectable({{ providedIn: 'root' }})
export class {controller.Name} extends EndpointBase {{
  constructor(
    http: HttpClient,
    auth: AuthService
  ) {{
    super(http, auth);
    this.endpointUrl = '/api/{controller.Name.Replace("Controller","")}'
  }}
  {string.Join("\r\n",angularServiceUnits.Select(x => ConvertToAngularServiceMethod(x)))}
  }}
        ";
        return serviceTs;
    }
    private string ConvertToAngularServiceMethod (AngularServiceUnit unit) {
        string routeArrStr = unit.Routes != null?string.Join (",", unit.Routes.Select (x => x.Name)): "";
        string QueryDicStr = unit.Queries != null?$"{{{string.Join(", ",unit.Queries.Select(x => x.Name))}}}": "null";
        string BodyStr = unit.Body != null?unit.Body.Name: "null";
        List<TypescriptParameter> attrs = new();
        if(unit.Routes!=null){
            attrs.AddRange(unit.Routes);
        }
        if(unit.Queries!=null){
            attrs.AddRange(unit.Queries);
        }
        if(unit.Body!=null){
            attrs.Add(unit.Body);
        }
        string attrStr = string.Join(",",attrs.Select(x => $"{x.Name}:{x.Type}"));

        if (unit.Type == MethodType.post) {
            return $@"public {unit.MethodName}({attrStr}): Observable<{{any}}> {{
    return this.httpPost(`${{this.endpointUrl}}`,[{routeArrStr}],{QueryDicStr},{BodyStr},this.requestHeaders);
  }}";
        } else if (unit.Type == MethodType.put) {
            return $@"public {unit.MethodName}({attrStr}): Observable<{{any}}> {{
    return this.httpPut(`${{this.endpointUrl}}`,[{routeArrStr}],{QueryDicStr},{BodyStr},this.requestHeaders);
  }}";
        } else if (unit.Type == MethodType.patch) {
            return $@"public {unit.MethodName}({attrStr}): Observable<{{any}}> {{
    return this.httpPatch(`${{this.endpointUrl}}`,[{routeArrStr}],{QueryDicStr},{BodyStr},this.requestHeaders);
  }}";
        } else if (unit.Type == MethodType.delete) {
            return $@"public {unit.MethodName}({attrStr}): Observable<{{any}}> {{
    return this.httpDelete(`${{this.endpointUrl}}`,[{routeArrStr}],{QueryDicStr},this.requestHeaders);
  }}";
        } else {
            return $@"public {unit.MethodName}({attrStr}): Observable<{{any}}> {{
    return this.httpGet(`${{this.endpointUrl}}`,[{routeArrStr}],{QueryDicStr},this.requestHeaders);
  }}";
        }
    }
    private IEnumerable<AngularServiceUnit> GetAngularServiceUnits (Type controller) {
        List<AngularServiceUnit> angularServiceUnits = new ();
        MethodInfo[] methods = controller.GetMethods ();
        foreach (MethodInfo method in methods) {
            if (method.ReturnType != typeof (IActionResult)) {
                continue;
            }
            AngularServiceUnit angularServiceUnit = new AngularServiceUnit ();
            angularServiceUnit.MethodName = method.Name;
            IEnumerable<Attribute> ? methodAttrs = method.GetCustomAttributes ();
            foreach (Attribute methodAttr in methodAttrs) {
                if (methodAttr.GetType () == typeof (HttpGetAttribute)) {
                    angularServiceUnit.Type = MethodType.get;
                }
                if (methodAttr.GetType () == typeof (HttpPostAttribute)) {
                    angularServiceUnit.Type = MethodType.post;
                }
                if (methodAttr.GetType () == typeof (HttpPutAttribute)) {
                    angularServiceUnit.Type = MethodType.put;
                }
                if (methodAttr.GetType () == typeof (HttpPatchAttribute)) {
                    angularServiceUnit.Type = MethodType.patch;
                }
                if (methodAttr.GetType () == typeof (HttpDeleteAttribute)) {
                    angularServiceUnit.Type = MethodType.delete;
                }
            }
            ParameterInfo[] parameters = method.GetParameters ();
            foreach (ParameterInfo parameter in parameters) {
                TypescriptParameter typescriptParameter = new TypescriptParameter () {
                    Name = parameter.Name,
                    Type = Utilities.ConvertToTypescriptType (parameter.ParameterType)
                };
                IEnumerable<CustomAttributeData> ? attrs = parameter.CustomAttributes;
                if (attrs != null) {
                    foreach (CustomAttributeData attr in attrs) {
                        if (attr.AttributeType == typeof (FromRouteAttribute)) {
                            angularServiceUnit.Routes.Add (typescriptParameter);
                        }
                        if (attr.AttributeType == typeof (FromQueryAttribute)) {
                            angularServiceUnit.Queries.Add (typescriptParameter);
                        }
                        if (attr.AttributeType == typeof (FromBodyAttribute)) {
                            angularServiceUnit.Body = typescriptParameter;
                        }
                    }
                }
            }
            angularServiceUnits.Add (angularServiceUnit);
        }
        return angularServiceUnits;
    }
    }
public class TypescriptParameter {
    public string Name { get; set; }
    public string Type { get; set; }
}
public class AngularServiceUnit {
    public MethodType Type { get; set; }
    public string MethodName { get; set; }
    public List<TypescriptParameter> ? Routes { get; set; } = new ();
    public List<TypescriptParameter> ? Headers { get; set; } = new ();
    public List<TypescriptParameter> ? Queries { get; set; } = new ();
    public TypescriptParameter? Body { get; set; }
}
public enum MethodType {
    get = 1,
    post = 2,
    put = 3,
    patch = 4,
    delete = 5
}