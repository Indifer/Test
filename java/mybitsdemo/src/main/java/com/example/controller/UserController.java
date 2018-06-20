package com.example.controller;


import com.example.mapper.UserMapper;
import com.example.model.User;
import com.example.repositorys.UserRepository;
import lombok.experimental.var;
import org.apache.ibatis.session.RowBounds;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import tk.mybatis.mapper.entity.Example;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;

import java.util.ArrayList;


@RestController
@RequestMapping("/user")
public class UserController {
    @Autowired
    private UserRepository userRepository;

    @RequestMapping(value = "/selectByPrimaryKey")
    public String selectByPrimaryKey(int id) {
        User user = userRepository.Get(id);
        if (user != null) {
            return user.toString();
        } else {
            return "null";
        }
    }

    @RequestMapping(value = "/selectOne")
    public String selectOne() {
        User userselect = new User();
        userselect.setUsername("2@qq.com").setSex(1);
        User user = userRepository.Get(userselect);
        if (user != null) {
            return user.toString();
        } else {
            return "null";
        }
    }

    @RequestMapping(value = "/selectOneByExample")
    public String selectOneByExample() {
        Example example = new Example(User.class);
        example.createCriteria()
                .andEqualTo("sex", 1)
                .andGreaterThan("id", 8)
                .andIn("nickname", new ArrayList<String>() {{
                    add("土豆-10");
                }});
        User user = userRepository.Get(example);
        if (user != null) {
            return user.toString();
        } else {
            return "null";
        }
    }

    @RequestMapping(value = "/selectAll")
    public String selectAll() {
        List<User> userlist = userRepository.GetList();
        return userlist.toString();
    }

    @RequestMapping(value = "/select")
    public String select() {
        User user = new User();
        user.setSex(2);
        List<User> userlist = userRepository.GetList(user);
        return userlist.toString();
    }

    @RequestMapping(value = "/selectByExample")
    public String selectByExample() {
        Example example = new Example(User.class);
        //返回的列
        example.selectProperties("id", "username", "sex");
        //查询条件
        example.createCriteria()
                .andEqualTo("sex", 2);
        example.or().orLessThan("id", 4).andCondition("username like '%@qq.com'");
        List<User> userlist = userRepository.GetList(example);
        return userlist.toString();
    }

    @RequestMapping(value = "/selectByExampleAndRowBounds")
    public String selectByExampleAndRowBounds() {
        Example example = new Example(User.class);
        example.createCriteria().andEqualTo("sex", 1);
        List<User> userlist = userRepository.GetList(example, new RowBounds(1, 5));
        List<User> userlist1 = new ArrayList<>();
        userlist1.addAll(userlist);
        return userlist1.toString();
    }

    @RequestMapping(value = "/selectByRowBounds")
    public String selectByRowBounds() {
        User user = new User();
        user.setSex(1);
        List<User> userlist = userRepository.GetList(user, new RowBounds(1, 5));
        List<User> userlist1 = new ArrayList<>();
        userlist1.addAll(userlist);
        return userlist1.toString();
    }

    @RequestMapping(value = "/insert")
    public String insert() {
        User user = new User();
        user.setSex(1);
        user.setUsername("小明");
        user.setNickname("小明明");
        user.setPassword("112233");
        user.setRegister_date(new Date());
        int id = userRepository.Insert(user);
        if (id == 1) {
            User userhas = userRepository.Get(user.getId());
            return userhas.toString();
        } else {
            return "插入失败";
        }
    }

    @RequestMapping(value = "/insertSelective")
    public String insertSelective() {
        User user = new User();
        user.setSex(1);
        user.setUsername("小明1");
        user.setPassword("123123");
        int id = userRepository.insertSelective(user);
        if (id == 1) {
            User userhas = userRepository.Get(user.getId());
            return userhas.toString();
        } else {
            return "插入失败";
        }
    }

    @RequestMapping(value = "/insertList")
    public String insertList() {
        List<User> userlist = new ArrayList<User>() {
            {
                add(new User("小红", "123123", "小红红", 2, new Date()));
            }

            {
                add(new User("小红1", "123123", "小红红1", 2, new Date()));
            }

            {
                add(new User("小红2", "123123", "小红红2", 2, new Date()));
            }

            {
                add(new User("小红3", "123123", "小红红3", 2, new Date()));
            }
        };
        userRepository.insert(userlist);
        return "sucess";
    }

    @RequestMapping(value = "/updateByPrimaryKey")
    public String updateByPrimaryKey() {
        User user = new User();
        user.setId(1);
        user.setUsername("hhhh");
        user.setPassword("56789");
        user.setRegister_date(new Date());
        userRepository.update(user);
        User userhas = userRepository.Get(user.getId());
        return userhas.toString();
    }

    @RequestMapping(value = "/updateByExample")
    public String updateByExample() {
        User user = new User();
        user.setUsername("hhhh");
        user.setPassword("56789");
        user.setRegister_date(new Date());
        Example example = new Example(User.class);
        example.createCriteria().andEqualTo("sex", 2);
        userRepository.update(user, example);
        return "sucess";
    }

    @RequestMapping(value = "/updateByExampleSelective")
    public String updateByExampleSelective() {
        User user = new User();
        user.setUsername("hhhh11111");
        Example example = new Example(User.class);
        example.createCriteria().andEqualTo("sex", 2);
        userRepository.updateSelective(user, example);
        return "sucess";
    }

    @RequestMapping(value = "/updateSelective")
    public String updateSelective() {
        User user = new User();
        user.setId(1);
        user.setUsername("hhhh11111");
        userRepository.updateSelective(user);
        return "sucess";
    }

    @RequestMapping(value = "/deleteByPrimaryKey")
    public String deleteByPrimaryKey() {
        userRepository.delete(17);
        return "sucess";
    }

    @RequestMapping(value = "/delete")
    public String delete() {
        User user = new User();
        user.setUsername("小明");
        userRepository.delete(user);
        return "sucess";
    }

    @RequestMapping(value = "/deleteByExample")
    public String deleteByExample() {
        Example example = new Example(User.class);
        example.createCriteria().andEqualTo("password", "112233");
        userRepository.delete(example);
        return "sucess";
    }

}
