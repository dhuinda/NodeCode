import '../styles/globals.css'
import type { AppProps } from 'next/app'
import { ChakraProvider } from '@chakra-ui/react'
import Head from 'next/head'
import theme from 'lib/theme'
import NavBar from 'components/NavBar'

function MyApp({ Component, pageProps }: AppProps) {
  return (
    <>
      <Head>
        <title>cdpm</title>
        <meta name='viewport' content='minimum-scale=1, initial-scale=1, width=device-width' />
      </Head>
      <ChakraProvider theme={theme}>
        <NavBar />
        <Component {...pageProps} />
      </ChakraProvider>
    </>
  )
}
export default MyApp
