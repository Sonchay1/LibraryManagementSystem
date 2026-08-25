using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class LibraryManagementSystemContext(DbContextOptions<LibraryManagementSystemContext> options) : IdentityDbContext<LibraryManagementSystem.Data.ApplicationUser>(options)
{
}
