package com.ranbo.aoplog.controller;

import com.ranbo.aoplog.aop.Log;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/test")
public class testController {
    @GetMapping("/getData")
    @Log
    public String getData() {
        System.out.println("run method");
        return "Hello Aop.";
    }
}
