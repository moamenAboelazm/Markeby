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

    public enum BoatStatus
    {
        Active,
        UnderMaintenance,
        OutOfService
    }

    public enum TripType
    {
        Snorkeling,
        Diving,
        Fishing,
        Party,
        Private
    }

    public enum TripStatus
    {
        Scheduled,
        Ongoing,
        Completed,
        Cancelled
    }

}
