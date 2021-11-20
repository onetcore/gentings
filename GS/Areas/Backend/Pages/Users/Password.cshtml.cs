using GS.Extensions.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace GS.Areas.Backend.Pages.Users
{
    /// <summary>
    /// ����û���
    /// </summary>
    public class PasswordModel : ModelBase
    {
        private readonly IUserManager _userManager;

        public PasswordModel(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public IActionResult OnGet(int id)
        {
            var user = _userManager.Find(id);
            if (user == null)
                return BadRequest();
            Input = new InputModel
            {
                UserId = user.Id,
                UserName = user.UserName
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindAsync(Input.UserId);
                if (user == null)
                    return Error("�û������ڣ�");
                user.Password = Extensions.Security.User.Hashed(user.UserName, Input.Password);
                user.UpdatedDate = DateTimeOffset.Now;
                var result = await _userManager.UpdateAsync(user.Id, new { user.Password, user.UpdatedDate });
                return Json(result, Gentings.Extensions.DataAction.Updated, "�û�����");
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
            /// �û�Id��
            /// </summary>
            public int UserId { get; set; }

            /// <summary>
            /// �û����ơ�
            /// </summary>
            [Required(ErrorMessage = "�û�������Ϊ�գ�")]
            public string UserName { get; set; }

            /// <summary>
            /// ȷ�����롣
            /// </summary>
            [Required(ErrorMessage = "ȷ�����벻��Ϊ�գ�")]
            [Compare("Password", ErrorMessage = "ȷ����������벻ƥ�䣡")]
            public string ConfirmPassword { get; set; }

            /// <summary>
            /// ���롣
            /// </summary>
            [Required(ErrorMessage = "���벻��Ϊ�գ�")]
            public string Password { get; set; }
        }
    }
}
