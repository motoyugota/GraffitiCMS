using System;
using System.Collections.Generic;
using System.Text;

namespace Graffiti.Core
{
    public static class GraffitiLicense
    {
        public static LicenseType GetCurrentLicenseType()
        {
            return LicenseType.Personal;
        }

        public static bool IsLicenseValid(LicenseType type)
        {
            LicenseType current = GetCurrentLicenseType();

            switch (current)
            {
                case LicenseType.Personal:

                    if(type == LicenseType.Personal)
                        return true;

                    break;

                case LicenseType.Commercial:

                    if (type == LicenseType.Personal || type == LicenseType.Commercial)
                        return true;

                    break;

                case LicenseType.Professional:

                    if (type == LicenseType.Personal || type == LicenseType.Commercial || type == LicenseType.Professional)
                    return true;

                    break;

                default:
                    return false;
            }

            return false;
        }
    }

    public enum LicenseType
    {
        Personal,
        Commercial, 
        Professional
    }
}
