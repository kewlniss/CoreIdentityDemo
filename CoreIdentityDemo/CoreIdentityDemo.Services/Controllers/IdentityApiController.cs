using CoreIdentityDemo.Services.Data.Entities;
using CoreIdentityDemo.Services.Models;
using CoreIdentityDemo.Services.Util;
using System;
using System.Linq;
using System.Web.Http;

namespace CoreIdentityDemo.Services.Controllers
{
    [RoutePrefix("api/identity")]
    public class IdentityApiController : BaseApiController
    {
        [PostRoute("user/{userId}/claim")]
        public IHttpActionResult AddUserClaim([FromUri]Guid userId, [FromBody]ClaimModel model)
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

                        return Ok();
                    }

                    ModelState.AddModelError("", $"Invalid {nameof(userId)}");
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            return BadRequest(ModelState);
        }

        [PostRoute("user/{userId}/login")]
        public IHttpActionResult AddUserLogin([FromUri]Guid userId, [FromBody]LoginModel model)
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


                        return Ok();
                    }

                    ModelState.AddModelError("", $"Invalid {nameof(userId)}");
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            return BadRequest(ModelState);
        }

        [PostRoute("user/{userId}/role")]
        public IHttpActionResult AddUserToRole([FromUri]Guid userId, [FromBody]string roleName)
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
                                if (!user.Roles.Contains(role))
                                {
                                    user.Roles.Add(role);
                                    Context.SaveChanges();
                                }

                                return Ok();
                            }
                        }

                        ModelState.AddModelError("", $"Missing or invalid {nameof(roleName)}");
                    }

                    ModelState.AddModelError("", $"Invalid {nameof(userId)}");
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            return BadRequest(ModelState);
        }

        [PostRoute("user")]
        public IHttpActionResult CreateUser([FromBody] UserModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = getUser(model);
                    Context.Users.Add(user);
                    Context.SaveChanges();

                    return Ok();
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            return BadRequest(ModelState);
        }

        [DeleteRoute("user/{userId}")]
        public IHttpActionResult DeleteUser([FromUri]Guid userId)
        {
            try
            {
                var user = Context.Users.Find(userId);
                if (user != null)
                {
                    Context.Users.Remove(user);
                    Context.SaveChanges();

                    return Ok();
                }

                ModelState.AddModelError("", $"Invalid {nameof(userId)}");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return BadRequest(ModelState);
        }

        [GetRoute("user/email")]
        public IHttpActionResult FindUserByEmail([FromUri]string normalizedEmail)
        {
            try
            {
                var user = Context.Users.FirstOrDefault(x => x.NormalizedEmail == normalizedEmail);
                var model = getUserModel(user);
                return Json(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [GetRoute("user/{userId}")]
        public IHttpActionResult FindUserById([FromUri] Guid userId)
        {
            try
            {
                var user = Context.Users.Find(userId);
                var model = getUserModel(user);

                if (model == null)
                    return NotFound();

                return Json(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [GetRoute("user/login")]
        public IHttpActionResult FindUserByLogin([FromUri] string loginProvider, [FromUri] string providerKey)
        {
            try
            {
                var user = Context.UserLogins
                    .Where(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey)
                    .Select(x => x.User)
                    .FirstOrDefault();

                var model = getUserModel(user);

                if (model == null)
                    return NotFound();

                return Json(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [GetRoute("user/name")]
        public IHttpActionResult FindByUserName([FromUri]string normalizedUserName)
        {
            try
            {
                var user = Context.Users.FirstOrDefault(x => x.NormalizedUserName == normalizedUserName);
                var model = getUserModel(user);

                if (model == null)
                    return NotFound();

                return Json(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [GetRoute("user/{userId}/claims")]
        public IHttpActionResult GetUserClaims([FromUri]Guid userId)
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

                    return Json(claims);
                }

                return BadRequest($"Invalid {nameof(userId)}");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [GetRoute("user/{userId}/logins")]
        public IHttpActionResult GetUserLogins([FromUri]Guid userId)
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

                    return Json(logins);
                }

                return BadRequest($"Invalid {nameof(userId)}");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [GetRoute("user/{userId}/roles")]
        public IHttpActionResult GetUserRoles([FromUri]Guid userId)
        {
            try
            {
                var user = Context.Users.Find(userId);
                if (user != null)
                {
                    var roles = user.Roles.Select(x => x.Name).ToArray();
                    return Json(roles);
                }

                return BadRequest($"Invalid {nameof(userId)}");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [GetRoute("user/{userId}/tokens")]
        public IHttpActionResult GetUserTokens([FromBody]Guid userId)
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

                    return Json(tokens);
                }

                return BadRequest($"Invalid {nameof(userId)}");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [GetRoute("user/claim")]
        public IHttpActionResult GetUsersForClaim([FromUri]string claimType, [FromUri]string claimValue)
        {
            try
            {
                var users = Context.UserClaims
                    .Where(x => x.Type == claimType && x.Value == claimValue)
                    .Select(x => x.User)
                    .Distinct()
                    .Select(x => getUserModel(x))
                    .ToArray();

                return Json(users);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [GetRoute("user/role")]
        public IHttpActionResult GetUsersInRole([FromUri]string roleName)
        {
            try
            {
                var users = Context.Roles
                    .Where(x => x.Name == roleName)
                    .SelectMany(x => x.Users)
                    .Distinct()
                    .Select(x => getUserModel(x))
                    .ToArray();

                return Json(users);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [DeleteRoute("user/{userId}/claim")]
        public IHttpActionResult RemoveUserClaim([FromUri]Guid userId, [FromUri]string claimType, [FromUri]string claimValue)
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
                        return Ok();
                    }

                    return BadRequest("Invalid claim");
                }

                return BadRequest($"Invalid {nameof(userId)}");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [DeleteRoute("user/{userId}/role")]
        public IHttpActionResult RemoveUserRole([FromUri]Guid userId, [FromUri]string roleName)
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

                        return Ok();
                    }

                    return BadRequest($"Invalid {nameof(roleName)}");
                }

                return BadRequest($"Invalid {nameof(userId)}");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [DeleteRoute("user/{userId}/login")]
        public IHttpActionResult RemoveUserLogin([FromUri]Guid userId, [FromUri]string loginProvider, [FromUri]string providerKey)
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
                        return Ok();
                    }
                }

                return BadRequest($"Invalid {nameof(userId)}");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [DeleteRoute("user/{userId}/token")]
        public IHttpActionResult RemoveUserToken([FromUri]Guid userId, [FromUri]string loginProvider, [FromUri]string name)
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

                        return Ok();
                    }
                }

                return BadRequest($"Invalid {nameof(userId)}");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [PutRoute("user/{userId}/claim")]
        public IHttpActionResult ReplaceUserClaim([FromUri]Guid userId, [FromBody]ReplaceUserClaimModel model)
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

                            return Ok();
                        }

                        ModelState.AddModelError("", $"Invalid claim");
                    }

                    ModelState.AddModelError("", $"Invalid {nameof(userId)}");
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            return BadRequest(ModelState);
        }

        [PostRoute("user/{userId}/token")]
        public IHttpActionResult SetUserToken([FromUri]Guid userId, [FromBody]TokenModel model)
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

                        return Ok();
                    }

                    ModelState.AddModelError("", $"Invalid {nameof(userId)}");
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            return BadRequest(ModelState);
        }

        [PutRoute("user")]
        public IHttpActionResult UpdateUser([FromBody]UserModel model)
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

                        return Ok();
                    }

                    ModelState.AddModelError("", "Invalid user");
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            return BadRequest(ModelState);
        }

        [PostRoute("role/{roleId}/claim")]
        public IHttpActionResult AddRoleClaim([FromUri]Guid roleId, [FromBody]ClaimModel model)
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

                        return Ok();
                    }

                    ModelState.AddModelError("", $"Invalid {nameof(roleId)}");
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            return BadRequest(ModelState);
        }

        [PostRoute("role")]
        public IHttpActionResult AddRole([FromBody] RoleModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var role = getRole(model);
                    Context.Roles.Add(role);
                    Context.SaveChanges();

                    return Ok();
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            return BadRequest(ModelState);
        }

        [DeleteRoute("role/{roleId}")]
        public IHttpActionResult DeleteRole([FromUri]Guid roleId)
        {
            try
            {
                var role = Context.Roles.Find(roleId);
                if(role != null)
                {
                    Context.Roles.Remove(role);
                    Context.SaveChanges();
                    return Ok();
                }

                return BadRequest($"Invalid {nameof(roleId)}");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [GetRoute("role/{roleId}")]
        public IHttpActionResult FindRoleById([FromUri]Guid roleId)
        {
            try
            {
                var role = Context.Roles.Find(roleId);
                var model = getRoleModel(role);

                if (model == null)
                    return NotFound();

                return Json(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [GetRoute("role/name")]
        public IHttpActionResult FindRoleByName([FromUri]string normalizedRoleName)
        {
            try
            {
                var role = Context.Roles.FirstOrDefault(x => x.NormalizedName == normalizedRoleName);
                var model = getRoleModel(role);

                if (model == null)
                    return NotFound();

                return Json(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [GetRoute("role/{roleId}/claims")]
        public IHttpActionResult GetRoleClaims([FromUri]Guid roleId)
        {
            try
            {
                var role = Context.Roles.Find(roleId);
                if(role != null)
                {
                    var claims = role.RoleClaims.Select(x => new ClaimModel { Type = x.Type, Value = x.Value }).ToArray();
                    return Json(claims);
                }

                return BadRequest($"Invalid {nameof(roleId)}");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [DeleteRoute("role/{roleId}/claim")]
        public IHttpActionResult RemoveRoleClaim([FromUri]Guid roleId, [FromUri]string claimType, [FromUri]string claimValue)
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
                        return Ok();
                    }
                }

                return BadRequest($"Invalid {nameof(roleId)}");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [PutRoute("role")]
        public IHttpActionResult UpdateRole([FromBody]RoleModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var role = Context.Roles.Find(model.Id);
                    if (role != null)
                    {
                        populateRole(role, model);
                        Context.SaveChanges();

                        return Ok();
                    }

                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }

                ModelState.AddModelError("", "Invalid role");
            }

            return BadRequest(ModelState);
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
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (user == null)
                throw new ArgumentNullException(nameof(user));

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
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (model == null)
                throw new ArgumentNullException(nameof(model));

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
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            if (model == null)
                throw new ArgumentNullException(nameof(model));

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
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            model.ConcurrencyStamp = role.ConcurrencyStamp;
            model.Id = role.Id;
            model.Name = role.Name;
            model.NormalizedName = role.NormalizedName;
        }
    }
}
