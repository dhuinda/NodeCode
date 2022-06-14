import type { NextPage } from 'next'
import Head from 'next/head'
import Image from 'next/image'
import { Box, Flex, Heading, SimpleGrid } from '@chakra-ui/react'
import PackagePreview from 'components/PackagePreview'

const Home: NextPage = () => {
  return (
    <Box>
      <Box className='moving-gradient' p={{ base: '5vh 10vw', xl: '15vh 10vw' }}>
        {/* random taglines idk */}
        <Heading color='white' fontSize='42pt' mt='5vh'>
          Simplify your code
        </Heading>
        <Heading color='white' fontStyle='normal' fontSize='18pt' mt='3vh'>
          Use Code Designer Package Manager to reduce the amount of code in your project.
        </Heading>
      </Box>

      {/* todo get data from API */}
      <SimpleGrid columns={{ sm: 1, xl: 2 }} m='15vh 10vw'>
        <Box>
          <Heading>Popular Packages</Heading>
          <Box pr='50px'>
            <PackagePreview name='linked-list' version='0.1.0' description='A simple linked list implementation' />
            <PackagePreview name='linked-list' version='0.1.0' description='A simple linked list implementation' />
            <PackagePreview name='linked-list' version='0.1.0' description='A simple linked list implementation' />
            <PackagePreview name='linked-list' version='0.1.0' description='A simple linked list implementation' />
            <PackagePreview name='linked-list' version='0.1.0' description='A simple linked list implementation' />
            <PackagePreview name='linked-list' version='0.1.0' description='A simple linked list implementation' />
          </Box>
        </Box>
        <Box>
          <Heading>New Packages</Heading>
          <Box>
            <PackagePreview name='hashmap' version='0.1.0' description='A simple hash map implementation' />
            <PackagePreview name='hashmap' version='0.1.0' description='A simple hash map implementation' />
            <PackagePreview name='hashmap' version='0.1.0' description='A simple hash map implementation' />
            <PackagePreview name='hashmap' version='0.1.0' description='A simple hash map implementation' />
            <PackagePreview name='hashmap' version='0.1.0' description='A simple hash map implementation' />
            <PackagePreview name='hashmap' version='0.1.0' description='A simple hash map implementation' />
          </Box>
        </Box>
      </SimpleGrid>
    </Box>
    // todo: add footer w/ github, statement that it's for TSA, creators, download link, etc
  )
}

export default Home
