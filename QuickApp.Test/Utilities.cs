namespace QuickApp.Test.Helper;
public static class Utilities{
    public static string lowerCaseFirstChar(string pascal){
        return pascal.Substring(0,1).ToLower() + pascal.Substring(1);
    }
    public static string ConvertToTypescriptType (Type type) {
        if (
            type == typeof (int) ||
            type == typeof (long) ||
            type == typeof (decimal) ||
            type == typeof (double) ||
            type == typeof (float)
        ) {
            return "number";
        } else if (type == typeof (string)) {
            return "string";
        } else if (type.IsClass) {
            return type.Name;
        } else{
            return "any";
        }
    }
}