package hello.controllers;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

@Controller
@RequestMapping("/user")
public class UserController extends BaseApiController {

    @RequestMapping("/get")
    public String get(String name, @RequestParam(defaultValue = "110") Long id) {
        return "hello " + name + ":" + id.toString();
    }

}
