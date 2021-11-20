using GS.Extensions.Security;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace GS.Areas.Backend.Pages.Users
{
    /// <summary>
    /// ����û���
    /// </summary>
    public class EditModel : ModelBase
    {
        private readonly IUserManager _userManager;

        public EditModel(IUserManager userManager)
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
                UserName = user.UserName,
                NickName = user.NickName,
                Email = user.Email,
                Avatar = user.Avatar
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
                user.NickName = Input.NickName;
                user.Email = Input.Email;
                user.Avatar = Input.Avatar;
                if (!await _userManager.IsDuplicatedAsync(user))
                {
                    var result = await _userManager.UpdateAsync(user);
                    return Json(result, Gentings.Extensions.DataAction.Updated, "�û�");
                }
                else
                {
                    ModelState.AddModelError("Input.NickName", "�ǳ��Ѿ����ڣ�");
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
            /// �û�Id��
            /// </summary>
            public int UserId { get; set; }

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
            /// ͷ��
            /// </summary>
            public string? Avatar { get; set; }

            /// <summary>
            /// �����ʼ���
            /// </summary>
            [DataType(DataType.EmailAddress)]
            public string? Email { get; set; }
        }
    }
}
