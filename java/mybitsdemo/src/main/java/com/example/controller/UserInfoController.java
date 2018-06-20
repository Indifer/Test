package com.example.controller;

import com.example.enums.GenderEnum;
import com.example.model.UserInfo;
import com.example.repositorys.UserInfoRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/userinfo")
public class UserInfoController {

    @Autowired
    UserInfoRepository userInfoRepository;

    @RequestMapping(value = "/selectByPrimaryKey")
    public String selectByPrimaryKey(int id) {
        UserInfo userinfo = userInfoRepository.Get(id);
        if (userinfo != null) {
            return userinfo.toString();
        } else {
            return "null";
        }
    }

    @RequestMapping(value = "/add")
    public String add() {
        UserInfo userinfo = new UserInfo();
        userinfo.setEmail("qq@123.com");
        userinfo.setEnabled(1);
        userinfo.setPassword("123");
        userinfo.setQq("1111111");
        userinfo.setTel("18721111111");
        userinfo.setUsername("qqtest");
        userinfo.setUsertype("1");
        userinfo.setGender(GenderEnum.Female);
        userInfoRepository.Insert(userinfo);
        return userinfo.toString();
    }

    @RequestMapping(value = "/addnew")
    public String addnew() {
        UserInfo userinfo = new UserInfo();
        userinfo.setEmail("qq@123.com");
        userinfo.setEnabled(1);
        userinfo.setPassword("123");
        userinfo.setQq("1111111");
        userinfo.setTel("18721111111");
        userinfo.setUsername("qqtest");
        userinfo.setUsertype("1");
        userinfo.setGender(GenderEnum.Female);
        userInfoRepository.insertnew(userinfo);
        return userinfo.toString();
    }

}
