using GS.Extensions.Security;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace GS.Areas.Backend.Pages.Users
{
    /// <summary>
    /// ����û���
    /// </summary>
    public class CreateModel : ModelBase
    {
        private readonly IUserManager _userManager;

        public CreateModel(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Input.NickName))
                    Input.NickName = Input.UserName;
                var user = new User
                {
                    UserName = Input.UserName,
                    NickName = Input.NickName,
                    Email = Input.Email,
                    Password = Extensions.Security.User.Hashed(Input.UserName, Input.Password)
                };
                if (!await _userManager.IsDuplicatedAsync(user))
                {
                    var result = await _userManager.CreateAsync(user);
                    return Json(result, Gentings.Extensions.DataAction.Created, "�û�");
                }
                else
                {
                    ModelState.AddModelError("Input.UserName", "�û����Ѿ����ڣ�");
                }
            }
            return Error();
        }

        /// <summary>
        /// ����ģ��ʵ������
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// ����ģ�����͡�
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// �û����ơ�
            /// </summary>
            [Required(ErrorMessage = "�û�������Ϊ�գ�")]
            public string UserName { get; set; }

            /// <summary>
            /// �ǳơ�
            /// </summary>
            public string? NickName { get; set; }

            /// <summary>
            /// ���롣
            /// </summary>
            [Required(ErrorMessage = "���벻��Ϊ�գ�")]
            public string Password { get; set; }

            /// <summary>
            /// ȷ�����롣
            /// </summary>
            [Required(ErrorMessage = "ȷ�����벻��Ϊ�գ�")]
            [Compare("Password", ErrorMessage = "�����ȷ�����벻ƥ�䣡")]
            public string ConfirmPassword { get; set; }

            /// <summary>
            /// �����ʼ���
            /// </summary>
            [DataType(DataType.EmailAddress)]
            public string? Email { get; set; }
        }
    }
}
