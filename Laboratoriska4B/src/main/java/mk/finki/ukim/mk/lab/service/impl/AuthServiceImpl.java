//package mk.finki.ukim.mk.lab.service.impl;
//
//import mk.finki.ukim.mk.lab.model.exceptions.InvalidArgumentsException;
//import mk.finki.ukim.mk.lab.model.exceptions.InvalidUserCredentialsException;
//
//import mk.finki.ukim.mk.lab.repository.jpa.UserRepository;
//import mk.finki.ukim.mk.lab.service.AuthService;
//import org.springframework.security.core.userdetails.User;
//import org.springframework.stereotype.Service;
//
//@Service
//public class AuthServiceImpl implements AuthService {
//
//    private final UserRepository userRepository;
//
//    public AuthServiceImpl(UserRepository userRepository) {
//        this.userRepository = userRepository;
//    }
//
//    @Override
//    public User login(String username, String password) {
//        if (username == null || username.isEmpty() || password == null || password.isEmpty()) {
//            throw new InvalidArgumentsException();
//        }
//        return userRepository.findByUsernameAndPassword(username, password)
//                .orElseThrow(InvalidUserCredentialsException::new);
//    }
//
//}
