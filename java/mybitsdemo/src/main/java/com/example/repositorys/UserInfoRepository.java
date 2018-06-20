package com.example.repositorys;

import com.example.mapper.UserInfoMapper;
import com.example.model.UserInfo;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import tkmallcoo.mybatis.repository.BaseRepository;

@Service
public class UserInfoRepository extends BaseRepository<UserInfo,UserInfoMapper> {
    @Autowired
    UserInfoMapper userInfoMapper;
    public int insertnew(UserInfo userInfo)
    {
       return userInfoMapper.insertnew(userInfo);
    }
}
