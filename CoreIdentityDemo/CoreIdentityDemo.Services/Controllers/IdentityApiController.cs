using CoreIdentityDemo.Common.IdentityApi;
using CoreIdentityDemo.Common.Util;
using CoreIdentityDemo.Services.Data.Entities;
using CoreIdentityDemo.Services.Util;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CoreIdentityDemo.Services.Controllers
{
    [RoutePrefix("api/identity")]
    public class IdentityApiController : BaseApiController
    {
        [PostRoute("user/{userId}/claim")]
        public HttpResponseMessage AddUserClaim([FromUri]Guid userId, [FromBody]ClaimModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = Context.Users.Find(userId);
                    if (user != null)
                    {
                        var claim = new UserClaim
                        {
                            Type = model.Type,
                            Value = model.Value,
                            UserId = user.Id
                        };

                        user.UserClaims.Add(claim);
                        Context.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                    ModelState.AddModelError("", $"Invalid {nameof(userId)}");
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [PostRoute("user/{userId}/login")]
        public HttpResponseMessage AddUserLogin([FromUri]Guid userId, [FromBody]LoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = Context.Users.Find(userId);
                    if (user != null)
                    {
                        var login = new UserLogin
                        {
                            LoginProvider = model.LoginProvider,
                            ProviderDisplayName = model.ProviderDisplayName,
                            ProviderKey = model.ProviderKey,
                        };
                        user.UserLogins.Add(login);
                        Context.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                    ModelState.AddModelError("", $"Invalid {nameof(userId)}");
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [PostRoute("user/{userId}/role")]
        public HttpResponseMessage AddUserToRole([FromUri]Guid userId, [FromBody]string roleName)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = Context.Users.Find(userId);
                    if (user != null)
                    {
                        if (!string.IsNullOrWhiteSpace(roleName))
                        {
                            var role = Context.Roles.FirstOrDefault(x => x.Name == roleName);
                            if (role != null)
                            {
                                if (user.Roles.Contains(role))
                                {
                                    user.Roles.Add(role);
                                    Context.SaveChanges();
                                }

                                return Request.CreateResponse(HttpStatusCode.OK);
                            }
                        }

                        ModelState.AddModelError("", $"Missing or invalid {nameof(roleName)}");
                    }

                    ModelState.AddModelError("", $"Invalid {nameof(userId)}");
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [PostRoute("user")]
        public HttpResponseMessage CreateUser([FromBody]UserModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = getUser(model);
                    Context.Users.Add(user);
                    Context.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [DeleteRoute("user/{userId}")]
        public HttpResponseMessage DeleteUser([FromUri]Guid userId)
        {
            try
            {
                var user = Context.Users.Find(userId);
                if (user != null)
                {
                    Context.Users.Remove(user);
                    Context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                ModelState.AddModelError("", $"Invalid {nameof(userId)}");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [GetRoute("user/email")]
        public HttpResponseMessage FindUserByEmail([FromUri]string normalizedEmail)
        {
            try
            {
                var user = Context.Users.FirstOrDefault(x => x.NormalizedEmail == normalizedEmail);
                var model = getUserModel(user);
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [GetRoute("user/{userId}")]
        public HttpResponseMessage FindUserById([FromUri] Guid userId)
        {
            try
            {
                var user = Context.Users.Find(userId);
                var model = getUserModel(user);
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [GetRoute("user/login")]
        public HttpResponseMessage FindUserByLogin([FromUri] string loginProvider, [FromUri] string providerKey)
        {
            try
            {
                var user = Context.UserLogins
                    .Where(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey)
                    .Select(x => x.User)
                    .FirstOrDefault();
                var model = getUserModel(user);
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [GetRoute("user/name")]
        public HttpResponseMessage FindByUserName([FromUri]string normalizedUserName)
        {
            try
            {
                var user = Context.Users.FirstOrDefault(x => x.NormalizedUserName == normalizedUserName);
                var model = getUserModel(user);
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [GetRoute("user/{userId}/claims")]
        public HttpResponseMessage GetUserClaims([FromUri]Guid userId)
        {
            try
            {
                var user = Context.Users.Find(userId);
                if (user != null)
                {
                    var claims = user.UserClaims
                        .Select(x => new ClaimModel
                        {
                            Type = x.Type,
                            Value = x.Value
                        }).ToArray();
                    return Request.CreateResponse(HttpStatusCode.OK, claims);
                }

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Invalid {nameof(userId)}");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [GetRoute("user/{userId}/logins")]
        public HttpResponseMessage GetUserLogins([FromUri]Guid userId)
        {
            try
            {
                var user = Context.Users.Find(userId);
                if (user != null)
                {
                    var logins = user.UserLogins
                        .Select(x => new LoginModel
                        {
                            LoginProvider = x.LoginProvider,
                            ProviderDisplayName = x.ProviderDisplayName,
                            ProviderKey = x.ProviderKey
                        }).ToArray();
                    return Request.CreateResponse(HttpStatusCode.OK, logins);
                }

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Invalid {nameof(userId)}");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [GetRoute("user/{userId}/roles")]
        public HttpResponseMessage GetUserRoles([FromUri]Guid userId)
        {
            try
            {
                var user = Context.Users.Find(userId);
                if (user != null)
                {
                    var roles = user.Roles.Select(x => x.Name).ToArray();
                    return Request.CreateResponse(HttpStatusCode.OK, roles);
                }

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Invalid {nameof(userId)}");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [GetRoute("user/{userId}/tokens")]
        public HttpResponseMessage GetUserTokens([FromBody]Guid userId)
        {
            try
            {
                var user = Context.Users.Find(userId);
                if (user != null)
                {
                    var tokens = user.UserTokens
                        .Select(x => new TokenModel
                        {
                            LoginProvider = x.LoginProvider,
                            Name = x.Name,
                            Value = x.Value
                        }).ToArray();
                    return Request.CreateResponse(HttpStatusCode.OK, tokens);
                }

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Invalid {nameof(userId)}");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [GetRoute("user/claim")]
        public HttpResponseMessage GetUsersForClaim([FromUri]string claimType, [FromUri]string claimValue)
        {
            try
            {
                var users = Context.UserClaims
                    .Where(x => x.Type == claimType && x.Value == claimValue)
                    .Select(x => x.User)
                    .Distinct()
                    .Select(x => getUserModel(x))
                    .ToArray();

                return Request.CreateResponse(HttpStatusCode.OK, users);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [GetRoute("user/role")]
        public HttpResponseMessage GetUsersInRole([FromUri]string roleName)
        {
            try
            {
                var users = Context.Roles
                    .Where(x => x.Name == roleName)
                    .SelectMany(x => x.Users)
                    .Distinct()
                    .Select(x => getUserModel(x))
                    .ToArray();

                return Request.CreateResponse(HttpStatusCode.OK, users);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [DeleteRoute("user/{userId}/claim")]
        public HttpResponseMessage RemoveUserClaim([FromUri]Guid userId, [FromUri]string claimType, [FromUri]string claimValue)
        {
            try
            {
                var user = Context.Users.Find(userId);
                if (user != null)
                {
                    var claim = user.UserClaims.Where(x => x.Type == claimType && x.Value == claimValue).FirstOrDefault();
                    if (claim != null)
                    {
                        Context.UserClaims.Remove(claim);
                        Context.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Invalid claim");
                }

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Invalid {nameof(userId)}");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [DeleteRoute("user/{userId}/role")]
        public HttpResponseMessage RemoveUserRole([FromUri]Guid userId, [FromUri]string roleName)
        {
            try
            {
                var user = Context.Users.Find(userId);
                if (user != null)
                {
                    var role = user.Roles.FirstOrDefault(x => x.Name == roleName);
                    if (role != null)
                    {
                        user.Roles.Remove(role);
                        Context.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Invalid {nameof(roleName)}");
                }

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Invalid {nameof(userId)}");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [DeleteRoute("user/{userId}/login")]
        public HttpResponseMessage RemoveUserLogin([FromUri]Guid userId, [FromUri]string loginProvider, [FromUri]string providerKey)
        {
            try
            {
                var user = Context.Users.Find(userId);
                if(user != null)
                {
                    var login = user.UserLogins.FirstOrDefault(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);
                    if(login != null)
                    {
                        Context.UserLogins.Remove(login);
                        Context.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Invalid {nameof(userId)}");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [DeleteRoute("user/{userId}/token")]
        public HttpResponseMessage RemoveUserToken([FromUri]Guid userId, [FromUri]string loginProvider, [FromUri]string name)
        {
            try
            {
                var user = Context.Users.Find(userId);
                if(user != null)
                {
                    var token = user.UserTokens.FirstOrDefault(x => x.LoginProvider == loginProvider && x.Name == name);
                    if(token != null)
                    {
                        Context.UserTokens.Remove(token);
                        Context.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Invalid {nameof(userId)}");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [PutRoute("user/{userId}/claim")]
        public HttpResponseMessage ReplaceUserClaim([FromUri]Guid userId, [FromBody]ReplaceUserClaimArgs model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var user = Context.Users.Find(userId);
                    if(user != null)
                    {
                        var claim = user.UserClaims.FirstOrDefault(x => x.Type == model.claimType && x.Value == model.claimValue);
                        if(claim != null)
                        {
                            claim.Type = model.newClaimType;
                            claim.Value = model.newClaimValue;
                            Context.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK);
                        }

                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Invalid claim");
                    }

                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Invalid {nameof(userId)}");
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [PostRoute("user/{userId}/token")]
        public HttpResponseMessage SetUserToken([FromUri]Guid userId, [FromBody]TokenModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var user = Context.Users.Find(userId);
                    if(user != null)
                    {
                        var token = user.UserTokens.FirstOrDefault(x => x.LoginProvider == model.LoginProvider && x.Name == model.Name);
                        if(token != null)
                        {
                            token.Value = model.Value;
                        }
                        else
                        {
                            token = new UserToken
                            {
                                LoginProvider = model.LoginProvider,
                                Name = model.Name,
                                Value = model.Value,
                                UserId = user.Id
                            };
                            user.UserTokens.Add(token);
                        }

                        Context.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Invalid {nameof(userId)}");
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [PutRoute("user")]
        public HttpResponseMessage UpdateUser([FromBody]UserModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var user = Context.Users.Find(model.Id);
                    if(user != null)
                    {
                        populateUser(user, model);
                        Context.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                    ModelState.AddModelError("", "Invalid user");
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [PostRoute("role/{roleId}/claim")]
        public HttpResponseMessage AddRoleClaim([FromUri]Guid roleId, [FromBody]ClaimModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var role = Context.Roles.Find(roleId);
                    if(role != null)
                    {
                        var claim = new RoleClaim
                        {
                            Type = model.Type,
                            Value = model.Value,
                            RoleId = role.Id
                        };

                        role.RoleClaims.Add(claim);
                        Context.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                    ModelState.AddModelError("", $"Invalid {nameof(roleId)}");
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [PostRoute("role")]
        public HttpResponseMessage AddRole([FromBody] RoleModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var role = getRole(model);
                    Context.Roles.Add(role);
                    Context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [DeleteRoute("role/{roleId}")]
        public HttpResponseMessage DeleteRole([FromUri]Guid roleId)
        {
            try
            {
                var role = Context.Roles.Find(roleId);
                if(role != null)
                {
                    Context.Roles.Remove(role);
                    Context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Invalid {nameof(roleId)}");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [GetRoute("role/{roleId}")]
        public HttpResponseMessage FindRoleById([FromUri]Guid roleId)
        {
            try
            {
                var role = Context.Roles.Find(roleId);
                var model = getRoleModel(role);
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [GetRoute("role/name")]
        public HttpResponseMessage FindRoleByName([FromUri]string normalizedRoleName)
        {
            try
            {
                var role = Context.Roles.FirstOrDefault(x => x.NormalizedName == normalizedRoleName);
                var model = getRoleModel(role);
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [GetRoute("role/{roleId}/claims")]
        public HttpResponseMessage GetRoleClaims([FromUri]Guid roleId)
        {
            try
            {
                var role = Context.Roles.Find(roleId);
                if(role != null)
                {
                    var claims = role.RoleClaims.Select(x => new ClaimModel { Type = x.Type, Value = x.Value }).ToArray();
                    return Request.CreateResponse(HttpStatusCode.OK, claims);
                }

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Invalid {nameof(roleId)}");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [DeleteRoute("role/{roleId}/claim")]
        public HttpResponseMessage RemoveRoleClaim([FromUri]Guid roleId, [FromUri]string claimType, [FromUri]string claimValue)
        {
            try
            {
                var role = Context.Roles.Find(roleId);
                if(role != null)
                {
                    var claim = role.RoleClaims.FirstOrDefault(x => x.Type == claimType && x.Value == claimValue);
                    if(claim != null)
                    {
                        role.RoleClaims.Remove(claim);
                        Context.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Invalid {nameof(roleId)}");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [PutRoute("role")]
        public HttpResponseMessage UpdateRole([FromBody]RoleModel model)
        {
            if(ModelState.IsValid)
            {
                var role = Context.Roles.Find(model.Id);
                if(role != null)
                {
                    populateRole(role, model);
                    Context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                ModelState.AddModelError("", "Invalid role");
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        private UserModel getUserModel(User user)
        {
            if (user == null)
                return default(UserModel);

            var model = new UserModel();
            populateUserModel(model, user);
            return model;
        }

        private void populateUserModel(UserModel model, User user)
        {
            ExceptionUtil.ThrowIfNull(model, nameof(model));
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            model.AccessFailedCount = user.AccessFailedCount;
            model.ConcurrencyStamp = user.ConcurrencyStamp;
            model.Email = user.Email;
            model.EmailConfirmed = user.EmailConfirmed;
            model.Id = user.Id;
            model.LockoutEnabled = user.LockoutEnabled;
            model.LockoutEnd = user.LockoutEnd;
            model.NormalizedEmail = user.NormalizedEmail;
            model.NormalizedUserName = user.NormalizedUserName;
            model.PasswordHash = user.PasswordHash;
            model.PhoneNumber = user.PhoneNumber;
            model.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            model.SecurityStamp = user.SecurityStamp;
            model.TwoFactorEnabled = user.TwoFactorEnabled;
            model.UserName = user.UserName;
        }

        private User getUser(UserModel model)
        {
            if (model == null)
                return default(User);

            var user = new User();
            populateUser(user, model);
            return user;
        }

        private void populateUser(User user, UserModel model)
        {
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            ExceptionUtil.ThrowIfNull(model, nameof(model));

            user.AccessFailedCount = model.AccessFailedCount;
            user.ConcurrencyStamp = model.ConcurrencyStamp;
            user.Email = model.Email;
            user.EmailConfirmed = model.EmailConfirmed;
            user.Id = model.Id;
            user.LockoutEnabled = model.LockoutEnabled;
            user.LockoutEnd = model.LockoutEnd;
            user.NormalizedEmail = model.NormalizedEmail;
            user.NormalizedUserName = model.NormalizedUserName;
            user.PasswordHash = model.PasswordHash;
            user.PhoneNumber = model.PhoneNumber;
            user.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
            user.SecurityStamp = model.SecurityStamp;
            user.TwoFactorEnabled = model.TwoFactorEnabled;
            user.UserName = model.UserName;
        }

        private Role getRole(RoleModel model)
        {
            if (model == null)
                return default(Role);

            var role = new Role();
            populateRole(role, model);
            return role;
        }

        private void populateRole(Role role, RoleModel model)
        {
            ExceptionUtil.ThrowIfNull(role, nameof(role));
            ExceptionUtil.ThrowIfNull(model, nameof(model));

            role.ConcurrencyStamp = model.ConcurrencyStamp;
            role.Id = model.Id;
            role.Name = model.Name;
            role.NormalizedName = model.NormalizedName;
        }

        private RoleModel getRoleModel(Role role)
        {
            if (role == null)
                return default(RoleModel);

            var model = new RoleModel();
            populateRoleModel(model, role);
            return model;
        }

        private void populateRoleModel(RoleModel model, Role role)
        {
            ExceptionUtil.ThrowIfNull(model, nameof(model));
            ExceptionUtil.ThrowIfNull(role, nameof(role));

            model.ConcurrencyStamp = role.ConcurrencyStamp;
            model.Id = role.Id;
            model.Name = role.Name;
            model.NormalizedName = role.NormalizedName;
        }
    }
}
