namespace projjjecttttt.Models
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        //i used list because I cant use For with IEnumerable
        public List<RoleViewModel> Roles { get; set; }
    }
}
