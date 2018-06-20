package hello.exception;

import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.ResponseBody;

@ControllerAdvice
public class MyControllerAdvice {

    @ResponseBody
    @ExceptionHandler(value = Exception.class)
    public String errorHandler(Exception ex) {
        return "Exception error";
    }

    @ResponseBody
    @ExceptionHandler(value = MyException.class)
    public String errorHandler(MyException ex) {
        return "MyException error";
    }
}
