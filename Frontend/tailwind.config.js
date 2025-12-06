module.exports = {
    content: [
        './Razor/**/*.html',
        './Razor/**/*.razor',
        './wwwroot/index.html',
    ],
    safelist: [
        {
            pattern: /(bg|text|from|to)-percentage-p(|1|2|3|4|5|6|7|8|9|10)0/,
            variants: ['before']
        }
    ],
    //darkMode: false, // or 'media' or 'class'
    theme: {
        // theme is defined in app.css
        extend: {},
    },
    plugins: [],
}
