import {
  Box,
  Button,
  CircularProgress,
  Divider,
  Flex,
  Heading,
  Img,
  useBreakpointValue,
  useToast,
  Grid,
  GridItem,
  IconButton,
  Tooltip
} from '@chakra-ui/react'
import { NextPage } from 'next'
import { useRouter } from 'next/router'
import { useEffect, useState } from 'react'
import PackagePreview from 'components/PackagePreview'
import { AddIcon } from '@chakra-ui/icons'

export interface UserPrincipalResponse {
  username: string
  id: string
  enabled: boolean
  accountNonLocked: boolean
  accountNonExpired: boolean
  credentialsNonExpired: boolean
  name: string
  avatarUrl: string
}

const AccountPage: NextPage = () => {
  const [account, setAccount] = useState<UserPrincipalResponse | null>(null)
  const router = useRouter()
  const toast = useToast()
  const isDesktopView = useBreakpointValue({ base: false, lg: true })

  const getAccount = async () => {
    const response = await fetch('/api/v1/users/user').catch(e => console.log(e))
    if (!response) {
      router.push('/api/v1/oauth2/code/github')
      return
    }
    if (!response.ok) {
      if (response.status === 403) {
        router.push('/api/v1/oauth2/code/github')
      } else {
        toast({
          title: 'Error',
          description: `Error fetching account data: ${response.statusText}`,
          status: 'error',
          duration: 9000,
          isClosable: true
        })
      }
    }
    const json = await response.json()
    console.log(json)
    setAccount(json)
  }

  useEffect(() => {
    getAccount()
  }, [])

  if (account === null) {
    return (
      <Flex justifyContent='center' alignItems='center'>
        <CircularProgress />
      </Flex>
    )
  }

  const { avatarUrl, name } = account

  return (
    <Grid templateColumns={{ base: 'repeat(1, 1fr)', md: 'repeat(4, 1fr)' }} padding={{ base: '3vh 10vw', xl: '8vh 10vw' }}>
      <GridItem>
        <Box width={{ base: '80vw', md: '17.5vw', xl: '13vw' }}>
          <Img src={avatarUrl} borderRadius='25%' w={{ base: '50%', lg: '80%' }} />
          <Heading w='100%' pt='1vh'>
            {name}
          </Heading>
          {isDesktopView && <Divider color='#d5d5d5' opacity='1' borderColor='rgba(0,0,0,0.1)' m='25px 0' />}
          <Button variant='outline' colorScheme='red' mt='5px'>
            Delete Account
          </Button>
        </Box>
        {!isDesktopView && <Divider color='#d5d5d5' opacity='1' borderColor='rgba(0,0,0,0.1)' mt='25px' />}
      </GridItem>
      <GridItem colSpan={{ base: 1, md: 3 }} pr={isDesktopView ? '10vw' : '0'} mt={isDesktopView ? 0 : '2vh'}>
        <Flex justifyContent='space-between'>
          <Heading>Packages</Heading>
          <Tooltip label='Create package'>
            <IconButton aria-label='Create package' icon={<AddIcon />} size='lg' onClick={() => router.push('/create')} />
          </Tooltip>
        </Flex>
        <Box>
          <PackagePreview shortDescription='Test' version='0.1.0' name='r-tree' isSmall />
          <PackagePreview shortDescription='Test' version='0.1.0' name='r-tree' isSmall />
          <PackagePreview shortDescription='Test' version='0.1.0' name='r-tree' isSmall />
          <PackagePreview shortDescription='Test' version='0.1.0' name='r-tree' isSmall />
          <PackagePreview shortDescription='Test' version='0.1.0' name='r-tree' isSmall />
          <PackagePreview shortDescription='Test' version='0.1.0' name='r-tree' isSmall />
          <PackagePreview shortDescription='Test' version='0.1.0' name='r-tree' isSmall />
          <PackagePreview shortDescription='Test' version='0.1.0' name='r-tree' isSmall />
          <PackagePreview shortDescription='Test' version='0.1.0' name='r-tree' isSmall />
          <PackagePreview shortDescription='Test' version='0.1.0' name='r-tree' isSmall />
          <PackagePreview shortDescription='Test' version='0.1.0' name='r-tree' isSmall />
        </Box>
      </GridItem>
    </Grid>
  )
}

export default AccountPage
