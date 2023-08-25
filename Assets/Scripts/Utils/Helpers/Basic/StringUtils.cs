public static class StringUtils
{
    // Get the name of the backing field of an auto property
    // using the property name
    public static string BackingField(this string propertyName)
    {
        return "<" + propertyName + ">k__BackingField";
    }
}
