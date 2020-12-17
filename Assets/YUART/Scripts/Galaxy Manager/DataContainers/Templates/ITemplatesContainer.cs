using System;

namespace YUART.Scripts.Galaxy_Manager.DataContainers.Templates
{
    public interface ITemplatesContainer<out T, in TS>
    where T : struct
    where TS : Enum
    {
        T GetTemplate(TS typeOfObject);
    }
}