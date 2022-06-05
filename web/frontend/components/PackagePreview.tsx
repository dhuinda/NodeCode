import { Box, Flex, Heading, Img, Text } from '@chakra-ui/react'
import Link from 'next/link'
import { FC } from 'react'

interface Props {
  name: string
  version: string
  shortDescription: string
}

const PackagePreview: FC<Props> = ({ name, version, shortDescription }) => {
  return (
    <Link href={`/${name}-v${version}`} passHref>
      <a>
        <Flex mt='10px' mr={{ sm: '0px', xl: '100px' }} border='2px solid #6e97b4' p='15px' w='100%' borderRadius='5px'>
          <Img src='/blue_cube.png' w='56px' h='56px'></Img>
          <Flex flexDir='column' justifyContent='center' ml='20px'>
            <Heading as='h6' fontWeight='normal' fontSize='24px'>
              {name} v{version}
            </Heading>
            <Text fontSize='14px' color='gray'>
              {shortDescription}
            </Text>
          </Flex>
        </Flex>
      </a>
    </Link>
  )
}

export default PackagePreview
