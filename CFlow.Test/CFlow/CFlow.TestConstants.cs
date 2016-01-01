namespace CFlow.Test
{
    public static class TestConstants
    {
        // cflow.exe --reverse --format=posix --no-brief --no-number
        static public string SimpleReverse = @"app_init: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 23>
    system_init: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 70>
        main: int (void), <c:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\led_flasher_main.c 6>

";
        // cflow.exe --reverse --format=posix --no-brief --no-number
        static public string ComplexReverse = @"Default_Handler: <>
    GCLK_Handler: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 33>
    SYSCTRL_Handler: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 42>
    PM_Handler: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 52>
    SysTick_Handler: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 61>
GCLK_Handler: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 33>
PM_Handler: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 52>
SYSCTRL_Handler: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 42>
SysTick_Handler: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 61>
app_init: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 23>
    system_init: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 70>
        main: int (void), <c:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\led_flasher_main.c 6>
delay_example: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 28>
delay_init: <>
    app_init: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 23>
        system_init: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 70>
            main: int (void), <c:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\led_flasher_main.c 6>
delay_ms: <>
    main: int (void), <c:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\led_flasher_main.c 6>
    delay_example: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 28>
gpio_set_pin_direction: <>
    system_init: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 70>
        main: int (void), <c:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\led_flasher_main.c 6>
gpio_set_pin_level: <>
    system_init: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 70>
        main: int (void), <c:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\led_flasher_main.c 6>
gpio_set_pin_mux: <>
    system_init: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 70>
        main: int (void), <c:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\led_flasher_main.c 6>
gpio_toggle_pin_level: <>
    main: int (void), <c:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\led_flasher_main.c 6>
handler: <>
    GCLK_Handler: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 33>
    SYSCTRL_Handler: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 42>
    PM_Handler: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 52>
    SysTick_Handler: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 61>
init_mcu: <>
    system_init: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 70>
        main: int (void), <c:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\led_flasher_main.c 6>
main: int (void), <c:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\led_flasher_main.c 6>
system_init: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 70>
    main: int (void), <c:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\led_flasher_main.c 6>

";

        // cflow.exe --format=posix --no-number
        static public string FourLevelGraphFlow = @"main: int (void), <c:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\led_flasher_main.c 6>
    system_init: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 70>
        init_mcu: <>
        gpio_set_pin_direction: <>
        gpio_set_pin_level: <>
        gpio_set_pin_mux: <>
        app_init: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 23>
            delay_init: <>
    delay_ms: <>
    gpio_toggle_pin_level: <>

";

        // cflow.exe --format=posix --no-number
        static public string TwoLevelGraphFlow = @"main: int (void), <c:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\led_flasher_main.c 6>
    system_init: void (void), <C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c 70>
    delay_ms: <>
    gpio_toggle_pin_level: <>

";

        static public string NoGraph = @"main: int (void), <c:\Users\Morten\Documents\Atmel Studio\7.0\GccApplication1\GccApplication1\main.cpp 14>

";

        // cflow.exe --reverse --format=posix --no-number
        static public string SimpleXref = @"main * c:\Users\Morten\Documents\Atmel Studio\7.0\GccApplication1\GccApplication1\main.cpp:14 int (void)
";

        // cflow.exe --reverse --format=posix --no-number
        static public string ComplexXref = @"Default_Handler   C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c:38
Default_Handler   C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c:48
Default_Handler   C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c:57
Default_Handler   C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c:66
GCLK_Handler * C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c:33 void (void)
PM_Handler * C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c:52 void (void)
SYSCTRL_Handler * C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c:42 void (void)
SysTick_Handler * C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c:61 void (void)
app_init * C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c:23 void (void)
app_init   C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c:88
delay_example * C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c:28 void (void)
delay_init   C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c:25
delay_ms   c:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\led_flasher_main.c:11
delay_ms   C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c:30
gpio_set_pin_direction   C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c:77
gpio_set_pin_level   C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c:79
gpio_set_pin_mux   C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c:86
gpio_toggle_pin_level   c:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\led_flasher_main.c:12
handler   C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c:36
handler   C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c:45
handler   C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c:55
handler   C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c:64
init_mcu   C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c:72
main * c:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\led_flasher_main.c:6 int (void)
system_init * C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c:70 void (void)
system_init   c:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\led_flasher_main.c:8
";

    }
}
