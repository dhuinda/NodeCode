import { extendTheme } from '@chakra-ui/react'

const theme = extendTheme({
  colors: {
    grayBg: '#f5f5f5',
    grayBorder: '#d7d7d7',
    brightBlue: '#3d64c7'
  },
  fonts: {
    heading: "'Noto Sans', Sans-Serif",
    body: "'Noto Sans', Sans-Serif"
  },
  styles: {
    global: {
      'html, body': {
        backgroundColor: '#f0f0f0'
      }
    }
  },
  config: {
    initialColorMode: 'light',
    useSystemColorMode: false
  }
})

export default theme
