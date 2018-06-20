package springconsulconsumetest.demo.service;

import org.springframework.cloud.openfeign.FeignClient;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;

@FeignClient(value = "getUsers")
public interface Users {

    @RequestMapping(method = RequestMethod.GET, value = "/home", consumes = "application/json")
    String getHome(@RequestParam("name") String name);
}
