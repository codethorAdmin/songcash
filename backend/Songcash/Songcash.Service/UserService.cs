using Songcash.Repository;

namespace Songcash.Service;

public class UserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<int> LoginOrRegister(string email)
    {
        return await _userRepository.InsertAndGetUserId(email);
    }
}
