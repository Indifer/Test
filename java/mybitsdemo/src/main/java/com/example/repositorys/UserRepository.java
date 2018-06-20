package com.example.repositorys;


import com.example.mapper.UserMapper;
import com.example.model.User;
import org.springframework.stereotype.Service;
import tkmallcoo.mybatis.repository.BaseRepository;
@Service
public class UserRepository extends BaseRepository<User,UserMapper> {



}
