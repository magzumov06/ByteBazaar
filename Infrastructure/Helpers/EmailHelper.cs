// using Domain.DTOs.EmailDto;
// using Infrastructure.Services.EmailServices;
// using MimeKit.Text;
//
// namespace Infrastructure.Helpers;
//
// public static class EmailHelper
// {
//     public static async Task SendLoginDetailsEmailAsync(
//         IEmailService emailService,
//         string email,
//         string username,
//         string password,
//         string userType)
//     {
//         var subject = "Маълумоти воридшавӣ ба система";
//
//         var message = $@"
//                 <p>Салом {username},</p>
//                 <p>Пароли шумо: {password}</p>
//                 <p>Шумо ба қайд гирифта шудед.</p>";
//
//         await emailService.SendEmail(
//             new EmailMessageDto(new[] { email }, subject, message),
//             TextFormat.Html);
//     }
//     
//     public static async Task SendResetPasswordCodeEmailAsync(
//         IEmailService emailService,
//         string email,
//         string code)
//     {
//         var subject = "Рамзи тасдиқ барои барқарорсозии рамз";
//         var message = $@"
//             <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
//                 <div style='background: linear-gradient(45deg, #FF512F, #DD2476); padding: 20px; border-radius: 10px 10px 0 0;'>
//                     <h1 style='color: white; margin: 0; text-align: center;'>Барқарорсозии рамз</h1>
//                 </div>
//                 <div style='background: #f8f9fa; padding: 20px; border-radius: 0 0 10px 10px;'>
//                     <p>Салом!</p>
//                     <p>Шумо дархости барқарорсозии рамзро пешниҳод кардед. Рамзи тасдиқ:</p>
//                     <div style='background: white; padding: 15px; border-radius: 5px; margin: 15px 0; text-align: center;'>
//                         <h2 style='color: #0056b3; letter-spacing: 5px;'>{code}</h2>
//                     </div>
//                     <p><strong>Диққат:</strong></p>
//                     <ul>
//                         <li>Рамзи тасдиқ танҳо 10 дақиқа эътибор дорад</li>
//                         <li>Агар шумо дархост накарда бошед, ин паёмро нодида гиред</li>
//                         <li>Рамзи тасдиқро бо касе мубодила накунед</li>
//                     </ul>
//                 </div>
//             </div>";
//
//         await emailService.SendEmail(
//             new EmailMessageDto(new[] { email }, subject, message),
//             TextFormat.Html);
//     }
// }
