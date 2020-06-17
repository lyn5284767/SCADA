using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    public class RolesType
    {
        short _id;
        public short ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        string _roleType;
        public string RoleType
        {
            get
            {
                return _roleType;
            }
            set
            {
                _roleType = value;
            }
        }

        string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public RolesType(short id, string roleType, string description)
        {
            _id = id;
            _roleType = roleType;
            _description = description;
        }

        public RolesType()
        {

        }
    }
}
