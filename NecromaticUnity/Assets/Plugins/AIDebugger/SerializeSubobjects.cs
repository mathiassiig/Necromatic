using System.Collections.Generic;

namespace AIDebugger
{
    public interface SerializeSubobjects
    {
        List<object> GetSerializableObjects();
        object GetCurrentObject();
    }
}