/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: 'class',
  content: [
    "./Views/**/*.cshtml",
    "./wwwroot/**/*.html",
    "./wwwroot/js/**/*.js"
  ],
  safelist: [
    'translate-x-0',
    'translate-x-full',
    'opacity-100',
    'opacity-0'
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          50: '#e3f2fd',
          100: '#bbdefb',
          200: '#90caf9',
          300: '#64b5f6',
          400: '#42a5f5',
          500: '#0d6efd',
          600: '#0a58ca',
          700: '#0842a0',
          800: '#062e70',
          900: '#041f4a',
        },
      },
      fontFamily: {
        sans: ['Segoe UI', 'Tahoma', 'Geneva', 'Verdana', 'sans-serif'],
      },
    },
  },
  plugins: [],
}
