namespace AnjeerMarket.Extensions;

public static class MapperExtension
{
    public static T MapTo<T>(this Object obj) where T : class, new()
    {
        var objType = obj.GetType();
        var objProperties = objType.GetProperties();

        var dtoType = typeof(T);
        var dtoProperties = dtoType.GetProperties();

        var dto = new T();

        foreach (var objProperty in objProperties)
        {
            if (dtoProperties.Any(dp => dp.Name == objProperty.Name))
            {
                var dtoProperty = dtoType.GetProperty(objProperty.Name);
                var value = objProperty.GetValue(obj, null);

                dtoProperty.SetValue(dto, value, null);
            }
        }

        return dto;
    }
}
