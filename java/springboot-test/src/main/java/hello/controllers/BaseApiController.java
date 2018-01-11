package hello.controllers;

import org.springframework.http.HttpRequest;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ResponseBody;

import javax.annotation.Resource;

@ResponseBody
public class BaseApiController {
    HttpRequest request;
}
