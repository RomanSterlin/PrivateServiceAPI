using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BadooAPI.Models
{
    public enum JsonTypes
    {
        Login = 0,
        GetEncounters = 1,
        SERVER_APP_STARTUP = 2,
        GetSearchSettings = 3,
        Vote = 4,
        UpdateAboutMe = 5,
        SaveLocation = 6,
        UploadPhoto = 7,
        RemoveImage = 8,
        MakeProfilePhoto = 9,   
        SearchLocations = 10,
        GetImages = 11,
        LoginAM = 12,
        LoginUS = 13,
        Like = 14
    }
}
