using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace simpleWebApp.Models
{
    public class DeviceDataSchedul
{
    public int id { get; set; }
    public int client_id { get; set; }
    public string name { get; set; } = string.Empty;
    public bool status { get; set; }
    public bool scheduler_active { get; set; }
    public string started { get; set; } = string.Empty;
}

public class OnlineSchedul
{
    public DeviceDataSchedul[] data { get; set; } = Array.Empty<DeviceDataSchedul>();
    public int count { get; set; }
}

public class OfflineSchedul
{
    public DeviceDataSchedul[] data { get; set; } = Array.Empty<DeviceDataSchedul>();
    public int count { get; set; }
}

public class ActiveSchedul
{
    public DeviceDataSchedul[] data { get; set; } = Array.Empty<DeviceDataSchedul>();
    public int count { get; set; }
}
}