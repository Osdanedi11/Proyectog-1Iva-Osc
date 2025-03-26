#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieSeriesManagement.Models.Entities;

namespace MovieSeriesManagement.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public string Username { get; set; }

        public string ProfilePictureUrl { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "El nombre es obligatorio")]
            [Display(Name = "Nombre")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "El apellido es obligatorio")]
            [Display(Name = "Apellido")]
            public string LastName { get; set; }

            [Phone]
            [Display(Name = "Número de teléfono")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Foto de perfil")]
            public IFormFile ProfilePicture { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            ProfilePictureUrl = user.ProfilePictureUrl;

            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"No se pudo cargar el usuario con ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"No se pudo cargar el usuario con ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Error inesperado al intentar establecer el número de teléfono.";
                    return RedirectToPage();
                }
            }

            // Actualizar FirstName y LastName
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;

            // Manejar la carga de la imagen de perfil
            if (Input.ProfilePicture != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "profiles");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Input.ProfilePicture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Asegurarse de que el directorio existe
                Directory.CreateDirectory(uploadsFolder);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.ProfilePicture.CopyToAsync(fileStream);
                }

                // Eliminar la imagen anterior si existe
                if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
                {
                    string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, user.ProfilePictureUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                user.ProfilePictureUrl = "/images/profiles/" + uniqueFileName;
            }

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Tu perfil ha sido actualizado";
            return RedirectToPage();
        }
    }
}

