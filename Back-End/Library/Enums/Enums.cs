using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Enums
{
    public enum GenderType 
    { 
        Male, 
        Female 
    }

    public enum NationalityType 
    { 
        Egyptian,
        Saudi,
        Moroccan,
        Tunisian,
        Algerian,
        Libyan,
        Sudanese,
        Another 
    }

    public enum RoleType
    {
        Admin, 
        User 
    }

    public enum CaptainStatus
    {
        OnMission,
        OnShoreLeave
    }

    public enum BoatStatus
    {
        AtSea,
        Available,
        OutOfService
    }

    public enum TripStatus
    {
        Scheduled,
        Ongoing,
        Completed,
        Cancelled
    }

    public enum TripType
    {
        Snorkeling,
        Diving,
        Fishing,
        Party,
        Private
    }

}
