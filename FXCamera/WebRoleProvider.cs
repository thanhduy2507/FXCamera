using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using FXCamera.Models;

namespace FXCamera
{
    public class WebRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            using(var db=  new FXCameraDbContext())
            {

            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            using(var db=new FXCameraDbContext())
            {
                var result = (from role in db.Roles select role.RoleName).ToArray();
                return result;
            }
           
        }

        public override string[] GetRolesForUser(string username)
        {
            using(var db=  new FXCameraDbContext())
            {
                var result = (from user in db.Users 
                              join role in db.Roles 
                              on user.RoleId equals role.Id where (user.UserName == username) 
                              select role.RoleName).ToArray();
                return result;
            }            
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}