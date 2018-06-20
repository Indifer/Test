package springeurekaclienttest.springeurekaclienttest.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.cloud.client.ServiceInstance;
import org.springframework.cloud.client.discovery.DiscoveryClient;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.List;
import java.util.UUID;

@Controller
@ResponseBody
@RequestMapping("/user")
public class UserController {

    @Autowired
    private DiscoveryClient discoveryClient;

    private String uuid;

    public UserController(){
        uuid = UUID.randomUUID().toString();
    }

    @RequestMapping("/getId")
    public String getId() {
        return uuid;
    }

    @RequestMapping("/getInstances")
    public List<ServiceInstance> getInstances(String str) {
        return discoveryClient.getInstances(str);
    }


}
